package ucl.group18.bodyscanner;

import android.app.PendingIntent;
import android.app.Service;
import android.content.Intent;
import android.os.IBinder;
import android.support.v4.app.NotificationCompat;

import java.util.Calendar;
import java.util.List;

import ucl.group18.bodyscanner.database.DataSource;
import ucl.group18.bodyscanner.model.MeasurementRequest;

/**
 * Created by Shubham on 19/04/2015.
 */
public class MeasurementPollService extends Service implements ServerConnect.ServerConnectCallback {

    DataSource ds;

    @Override
    public IBinder onBind(Intent intent) {
        return null;
    }

    @Override
    public void onCreate() {

    }

    @Override
    public void onDestroy() {
        ds.close();
    }

    @Override
    public int onStartCommand(Intent intent, int flags, int startId) {

        ds = new DataSource(getApplicationContext());
        ds.open();

        List<MeasurementRequest> unProcessedMR = ds.getAllUnProcessedMeasurementRequests();

        for (MeasurementRequest measurementRequest : unProcessedMR){
            ServerConnect serverConnect = new ServerConnect(getApplicationContext());
            serverConnect.getMeasurementsFromServerAsync(measurementRequest, this);
        }

        return START_STICKY;
    }

    private void stopService() {

    }

    @Override
    public void getMeasurementCallback(MeasurementRequest measurementRequest) {

        if (measurementRequest.isProcessed()){
            //measurementRequest.
        }
        measurementRequest.setLastRequest(Calendar.getInstance());
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

}
