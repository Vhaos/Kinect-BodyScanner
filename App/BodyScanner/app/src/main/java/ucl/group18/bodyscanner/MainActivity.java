package ucl.group18.bodyscanner;

import android.content.Intent;
import android.net.Uri;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Toast;

import java.util.Calendar;
import java.util.List;

import ucl.group18.bodyscanner.database.DataSource;
import ucl.group18.bodyscanner.model.Measurement;
import ucl.group18.bodyscanner.model.MeasurementRequest;


public class MainActivity extends ActionBarActivity {

    private static final String LOG_TAG = "MainActivity";

    DataSource ds;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);


        ds = new DataSource(getApplicationContext());
        ds.open();

         /* EXPERIMENTAL PURPOSES
        List<MeasurementRequest> requests =  ds.getAllMeasurementRequests();




        ds.deleteMeasurementRequest(requests.get(1));
        ds.deleteMeasurementRequest(requests.get(2));


        MeasurementRequest request = requests.get(0);
        request.setProcessed(true);

        Measurement measurement = new Measurement();
        measurement.setMeasurements(3.0,25.3,12.1,25.0,12.2);

        request.setMeasurement(measurement);

        ds.updateMeasurementRequest(request);
        */

    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            Intent settingsIntent = new Intent(this, SettingsActivity.class);
            startActivity(settingsIntent);
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
    public void fabPressed(View view){



       launchBarCodeScanner();

        /* EXPERIMENTAL PURPOSES
         List<MeasurementRequest> requests =  ds.getAllMeasurementRequests();
        Log.v(LOG_TAG, "No of Requests: " + requests.size());

        for (MeasurementRequest request : requests){

            Log.v(LOG_TAG, "Request ID: " + request.getRequestID());
            Log.v(LOG_TAG, "Processed: " + request.isProcessed());
            Log.v(LOG_TAG, "Last Request: " + request.getLastRequest().toString());
            Log.v(LOG_TAG, "Measurement: " + request.getMeasurements().toString());
            Log.v(LOG_TAG, "=============END OF REQUEST============");


        }
         */




    }

    @Override
    protected void onDestroy(){
        ds.close();
    }

    private void launchBarCodeScanner() {

        try{
            Intent intent = new Intent("com.google.zxing.client.android.SCAN");
            intent.putExtra("SCAN_MODE", "QR_CODE_MODE");
            intent.putExtra("SAVE_HISTORY", false);
            startActivityForResult(intent, 0);
        }catch (Exception e){
            e.printStackTrace();
            Uri marketUri = Uri.parse("market://details?id=com.google.zxing.client.android");
            Intent marketIntent = new Intent(Intent.ACTION_VIEW,marketUri);
            Toast.makeText(this, getString(R.string.download_app_message),Toast.LENGTH_LONG).show();
            startActivity(marketIntent);
        }

    }

    /*
    Intent Callback method (for when QR scanner returns)
     */
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);
        if (requestCode == 0) {
            if (resultCode == RESULT_OK) {
                String qr_text = data.getStringExtra("SCAN_RESULT"); //this is the result
                Toast.makeText(this, qr_text,Toast.LENGTH_LONG).show();
            }
        }
    }
}
