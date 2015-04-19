package ucl.group18.bodyscanner;

import android.app.NotificationManager;
import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.IBinder;
import android.util.Log;
import android.widget.Toast;

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




    }
}
