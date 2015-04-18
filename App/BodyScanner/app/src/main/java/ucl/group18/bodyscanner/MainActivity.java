package ucl.group18.bodyscanner;

import android.content.Intent;
import android.net.Uri;
import android.os.AsyncTask;
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

        ServerConnect network = new ServerConnect(getApplicationContext());
        final MeasurementRequest mr = new MeasurementRequest("5522e70fe6cb4");

        network.getMeasurementsFromServerAsync(mr, new ServerConnect.ServerConnectCallback() {
            @Override
            public void getMeasurementCallback(Measurement m) {

                Log.v(LOG_TAG, "RESULT: " + m.toString());
                if (m != null){
                    mr.setMeasurement(m);
                    mr.setProcessed(true);
                    Log.v(LOG_TAG,"MEASUREMENTS: " + mr.getMeasurements().toString());
                }else{
                    Log.wtf(LOG_TAG, "Seriously?");
                }
        }
        });




    }

    @Override
    protected void onDestroy(){
        super.onDestroy();
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
