package ucl.group18.bodyscanner.cloudconnection;

import android.app.AlarmManager;
import android.app.IntentService;
import android.app.Notification;
import android.app.NotificationManager;
import android.app.PendingIntent;
import android.content.Context;
import android.content.Intent;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import java.util.Calendar;
import java.util.List;

import ucl.group18.bodyscanner.Logger;
import ucl.group18.bodyscanner.R;
import ucl.group18.bodyscanner.SharedPrefsHandler;
import ucl.group18.bodyscanner.MyMeasurementActivity;
import ucl.group18.bodyscanner.database.DataSource;
import ucl.group18.bodyscanner.model.MeasurementRequest;

/**
 * Created by Shubham on 19/04/2015.
 */
public class MeasurementPollService extends IntentService implements ServerConnect.ServerConnectCallback {

    private static final String LOG_TAG = "MeasurementPollService";
    DataSource ds;
    private static final int maxRequests = 8;
    private static final int timer = 30; // Seconds

    Logger logger = new Logger(this);

    boolean alarmAlreadySet = false;

    /**
     * Creates an IntentService for polling data from server
     */
    public MeasurementPollService() {
        super(LOG_TAG);
    }

    @Override
    protected void onHandleIntent(Intent intent) {

        Log.v(LOG_TAG, "MeasurementPollService starting...");
        logger.write( "MeasurementPollService starting...");



        alarmAlreadySet = false;

        if (!ServerConnect.isNetworkAvailable(getApplicationContext())){
            setAlarm(3600); //Try again in an hour
            logger.write( "No Internet connection...");
            return;
        }

        ds = new DataSource(getApplicationContext());
        List<MeasurementRequest> unProcessedMR = ds.getAllUnProcessedMeasurementRequests();

        Log.v(LOG_TAG, "No. of unprocessed MR: " + unProcessedMR.size());
        logger.write( "No. of unprocessed MR: " + unProcessedMR.size());

        for (MeasurementRequest measurementRequest : unProcessedMR){

            if (measurementRequest.getNoOfRequests() < maxRequests){
                Log.v(LOG_TAG, "Polling MR, ID: " + measurementRequest.getRequestID());
                logger.write( "Polling MR, ID: " + measurementRequest.getRequestID());
                measurementRequest.setNoOfRequests(measurementRequest.getNoOfRequests() +1);
                ServerConnect serverConnect = new ServerConnect(getApplicationContext());
                serverConnect.getMeasurementsFromServerAsync(measurementRequest, this);
            }

        }

    }



    @Override
    public void getMeasurementCallback(MeasurementRequest measurementRequest) {

        if (measurementRequest.isProcessed()){
            // Creating a Shallow copy because in incognito mode this information will be deleted
            // and measurement set to null. Which will cause the notification to fail.
           notifyUser(measurementRequest.shallowCopy());
        }else{
            logger.write("Server hasn't processed for ID: " + measurementRequest.getRequestID());

            if (measurementRequest.getNoOfRequests() < maxRequests ){

                if (alarmAlreadySet == false){
                    setAlarm(timer);
                }else{
                    logger.write("Alarm is already set");
                }


            }else{
                logger.write(measurementRequest.getRequestID() + " , " +
                        measurementRequest.getId() + " :" + "has surpassed max requests");
            }

        }


        measurementRequest.setLastRequest(Calendar.getInstance());

        SharedPrefsHandler prefs = new SharedPrefsHandler(getApplicationContext());
        if (prefs.isIncognitoMode()){ //Remove measurement Info if in incognito mode
            measurementRequest.setMeasurement(null);
            measurementRequest.setProcessed(false);
        }

        ds.updateMeasurementRequest(measurementRequest);

    }


    private void notifyUser(MeasurementRequest measurementRequest){

        Log.v(LOG_TAG, "Notifying User");
        logger.write( "Notifying User");

        Intent resultIntent = new Intent(this, MyMeasurementActivity.class);
        resultIntent.putExtra("measurementRequest", measurementRequest);
        PendingIntent resultPendingIntent =
                PendingIntent.getActivity(
                        this,
                        0,
                        resultIntent,
                        PendingIntent.FLAG_CANCEL_CURRENT
                );


        NotificationCompat.Builder mBuilder =
                        new NotificationCompat.Builder(this)
                        .setSmallIcon(R.drawable.ic_qr_scanner_icon)
                        .setContentTitle("Your Measurements are Ready!")
                        .setContentIntent(resultPendingIntent);

        Notification notification = mBuilder.build();
        notification.flags |= Notification.FLAG_AUTO_CANCEL;

        NotificationManager notificationManager = (NotificationManager) getSystemService(Context.NOTIFICATION_SERVICE);
        notificationManager.notify(0, notification);

    }

    /**
     * Sets an alarm for MeasurementPollService to be awaken
     * @param duration - The duration until alarm in seconds
     */
    private void setAlarm(int duration){

        Calendar cal = Calendar.getInstance();
        cal.add(Calendar.SECOND, duration);

        Log.v(LOG_TAG, "Setting Alarm in " + timer + "  seconds");
        logger.write( "Setting Alarm in " + timer + "  seconds");

        Intent intent = new Intent(MeasurementPollService.this, MeasurementPollService.class);
        PendingIntent pIntent = PendingIntent.getService(MeasurementPollService.this, 0, intent, 0);
        AlarmManager alarm = (AlarmManager)getSystemService(Context.ALARM_SERVICE);
        alarm.set(AlarmManager.RTC_WAKEUP,  cal.getTimeInMillis(), pIntent);
        alarmAlreadySet = true;
    }



}
