package ucl.group18.bodyscanner;

import android.content.Intent;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.MotionEvent;
import android.view.View;
import android.widget.ListView;

import java.util.List;

import ucl.group18.bodyscanner.database.DataSource;
import ucl.group18.bodyscanner.model.Measurement;
import ucl.group18.bodyscanner.model.MeasurementRequest;


public class MeasurementHistoryActivity extends ActionBarActivity implements View.OnClickListener {

    private static final String LOG_TAG ="HistoryActivity" ;
    RecyclerView recyclerView;
    private List<MeasurementRequest> measurementRequestList;
    MeasurementHistoryAdapter adapter;
    DataSource ds;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_measurement_history);
        recyclerView = (RecyclerView)findViewById(R.id.recyclerView);
        getSupportActionBar().setTitle(getResources().getString(R.string.measurement_history));
        Log.v(LOG_TAG, "MeasurementHistoryActivity Started");

        ds = new DataSource(this);
        Log.v(LOG_TAG, "DS Initialised");
        measurementRequestList = ds.getAllMeasurementRequests();
        Log.v(LOG_TAG, "Request List length: " + measurementRequestList.size());
        adapter = new MeasurementHistoryAdapter(this,measurementRequestList);
        recyclerView.setAdapter(adapter);


        final LinearLayoutManager layoutManager = new LinearLayoutManager(this);
        layoutManager.setOrientation(LinearLayoutManager.VERTICAL);
        recyclerView.setLayoutManager(layoutManager);

        adapter.notifyDataSetChanged();

    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        return true;
    }

    @Override
    public void onDestroy(){
        super.onDestroy();
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    @Override
    public void onClick(View v) {

        long id = (long) v.getTag();
        Log.v(LOG_TAG, "Deleting: " + id);



    }
}
