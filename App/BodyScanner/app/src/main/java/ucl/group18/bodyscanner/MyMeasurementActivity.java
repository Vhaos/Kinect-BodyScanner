package ucl.group18.bodyscanner;

import android.app.job.JobInfo;
import android.app.job.JobParameters;
import android.app.job.JobService;
import android.content.Intent;
import android.net.Uri;
import android.os.AsyncTask;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentTransaction;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.FrameLayout;
import android.widget.TextView;
import android.widget.Toast;

import java.util.Calendar;
import java.util.List;

import ucl.group18.bodyscanner.database.DataSource;
import ucl.group18.bodyscanner.fragments.MeasurementFragment;
import ucl.group18.bodyscanner.model.Measurement;
import ucl.group18.bodyscanner.model.MeasurementRequest;


public class MyMeasurementActivity extends ActionBarActivity implements MeasurementFragment.OnFragmentInteractionListener {

    private static final String LOG_TAG = "MainActivity";

    FrameLayout rootView;
    TextView no_measurements;
    DataSource ds;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Log.v(LOG_TAG, "My Measurement Activity Started");

        ds = new DataSource(getApplicationContext());
        ds.open();


        //Adding Fake Measurement Request
        MeasurementRequest mr1 = new MeasurementRequest("23io2i3ng2", MeasurementRequest.Gender.MALE);
        mr1.setProcessed(true);
        mr1.setLastRequest(Calendar.getInstance());
        Measurement measurement = new Measurement();
        measurement.setMeasurements(178.4,85.3,84.2,86.5,120.3);
        mr1.setMeasurement(measurement);
        mr1.setNoOfRequests(2);
        ds.addMeasurementRequest(mr1);
        ds.updateMeasurementRequest(mr1);

        //List<MeasurementRequest> lmr = ds.getAllMeasurementRequests();
        //Log.v(LOG_TAG, String.valueOf(lmr.size()));

        rootView = (FrameLayout) findViewById(R.id.root_view);
        no_measurements = (TextView)findViewById(R.id.no_measurement_text);



        if (getIntent().getParcelableExtra("measurementRequest") == null){
            MeasurementRequest mr = ds.getLatestProcessedMeasurementRequests();
            if (mr != null){
                no_measurements.setVisibility(View.GONE);

                Fragment newFragment = MeasurementFragment.newInstance(mr.getMeasurements());
                FragmentTransaction ft =  getSupportFragmentManager().beginTransaction();
                ft.add(R.id.root_view, newFragment).commit();

            }else{
                no_measurements.setVisibility(View.VISIBLE);
            }

        }

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

        Log.e(LOG_TAG, "Fab FUCKING pressed");

        //launchBarCodeScanner();

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

    @Override
    public void onFragmentInteraction(Uri uri) {

    }
}
