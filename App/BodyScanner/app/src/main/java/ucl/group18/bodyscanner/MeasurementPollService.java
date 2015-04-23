package ucl.group18.bodyscanner;

import android.app.AlarmManager;
import android.app.IntentService;
import android.app.PendingIntent;
import android.app.Service;
import android.content.Context;
import android.content.Intent;
import android.net.ConnectivityManager;
import android.net.NetworkInfo;
import android.os.IBinder;
import android.support.v4.app.NotificationCompat;
import android.util.Log;

import java.util.Calendar;
import java.util.List;

import ucl.group18.bodyscanner.database.DataSource;
import ucl.group18.bodyscanner.model.MeasurementRequest;

/**
 * Created by Shubham on 19/04/2015.
 */
public class MeasurementPollService extends IntentService implements ServerConnect.ServerConnectCallback {

    private static final String LOG_TAG = "MeasurementPollService";
    DataSource ds;

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

        if (!ServerConnect.isNetworkAvailable(getApplicationContext())){
            setAlarm(3600); //Try again in an hour
            return;
        }

        ds = new DataSource(getApplicationContext());
        List<MeasurementRequest> unProcessedMR = ds.getAllUnProcessedMeasurementRequests();

        Log.v(LOG_TAG, "No. of unprocessed MR: " + unProcessedMR.size());

        for (MeasurementRequest measurementRequest : unProcessedMR){

            if (measurementRequest.getNoOfRequests() < 3){
                Log.v(LOG_TAG, "Polling MR, ID: " + measurementRequest.getRequestID());
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
        }else {

            if (measurementRequest.getNoOfRequests() < 3 && alarmAlreadySet == true){
                Log.v(LOG_TAG, "Setting Alarm in 180 seconds");
                setAlarm(180);
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

        NotificationCompat.Builder mBuilder =
                        new NotificationCompat.Builder(this)
                        .setSmallIcon(R.drawable.ic_qr_scanner_icon)
                        .setContentTitle("Your Measurements are Ready!");


        Intent resultIntent = new Intent(this, MyMeasurementActivity.class);
        resultIntent.putExtra("measurementRequest", measurementRequest);
        // Because clicking the notification opens a new ("special") activity, there's
        // no need to create an artificial back stack.
        PendingIntent resultPendingIntent =
                PendingIntent.getActivity(
                        this,
                        0,
                        resultIntent,
                        PendingIntent.FLAG_UPDATE_CURRENT
                );

    }

    /**
     * Sets an alarm for MeasurementPollService to be awaken
     * @param duration - The duration until alarm in seconds
     */
    private void setAlarm(int duration){
        Intent intent = new Intent(MeasurementPollService.this, MeasurementPollService.class);
        PendingIntent pIntent = PendingIntent.getService(MeasurementPollService.this, 0, intent, 0);
        AlarmManager alarm = (AlarmManager)getSystemService(Context.ALARM_SERVICE);
        alarm.set(AlarmManager.RTC_WAKEUP, duration*1000, pIntent);
        alarmAlreadySet = true;
    }



}
