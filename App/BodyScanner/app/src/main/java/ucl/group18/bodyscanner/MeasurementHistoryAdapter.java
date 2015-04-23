package ucl.group18.bodyscanner;

import android.app.Activity;
import android.content.Context;
import android.content.Intent;
import android.graphics.Color;
import android.support.v7.widget.RecyclerView;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;
import android.widget.TextView;
import android.widget.Toast;

import java.text.DateFormat;
import java.text.FieldPosition;
import java.text.ParsePosition;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;

import ucl.group18.bodyscanner.database.DataSource;
import ucl.group18.bodyscanner.model.Measurement;
import ucl.group18.bodyscanner.model.MeasurementRequest;

/**
 * Created by Shubham on 22/04/2015.
 */

public class MeasurementHistoryAdapter extends RecyclerView.Adapter<MeasurementHistoryAdapter.ViewHolder>{

    public static final int IdTag = 0;

    final String LOG_TAG = "MeasurementHistory";
    List<MeasurementRequest> measurementRequests;
    int rowLayout;
    Context context;
    MeasurementHistoryActivity activity;


    public MeasurementHistoryAdapter(MeasurementHistoryActivity activity, List<MeasurementRequest> measurementRequests){
        this.measurementRequests = measurementRequests;
        this.rowLayout = R.layout.measurement_request_hisory_row;
        this.context = activity;
        Log.v(LOG_TAG, "MeasurementHistoryAdapter initialised");
    }

    @Override
    public ViewHolder onCreateViewHolder(ViewGroup parent, int viewType) {
        View v = LayoutInflater.from(parent.getContext()).inflate(rowLayout, parent, false);
        return new ViewHolder(v);
    }

    @Override
    public void onBindViewHolder(ViewHolder holder, int position) {

        final MeasurementRequest measurementRequest = measurementRequests.get(position);

        SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy/MM/dd HH:mm:ss");

        holder.itemView.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {

                if (measurementRequest.isProcessed()) {

                    Intent intent = new Intent(context,
                            MyMeasurementActivity.class);
                    intent.putExtra(MyMeasurementActivity.INTENT_EXTRA_ID,
                            measurementRequest);
                    context.startActivity(intent);
                }else{
                    Toast.makeText(context,context.getString(R.string.no_measurements_stored),Toast.LENGTH_SHORT).show();
                }

            }
        });

        holder.dateFirstCreated.setText(dateFormat.format(
                measurementRequest.getFirstCreated().getTime()));

        if (!measurementRequest.isProcessed()){
            holder.dateFirstCreated.setTextColor(Color.LTGRAY);
        }else{
            holder.dateFirstCreated.setTextColor(Color.DKGRAY);
        }

        holder.button.setTag(measurementRequest.getId());
        holder.button.setOnClickListener(activity);
        holder.button.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                DataSource ds = new DataSource(context);

                ds.deleteMeasurementRequest(measurementRequest.getId());
                int position = measurementRequests.indexOf(measurementRequest);
                measurementRequests.remove(position);
                notifyItemRemoved(position);
            }
        });
    }

    @Override
    public int getItemCount() {
        return measurementRequests == null ? 0 : measurementRequests.size();
    }

    public static class ViewHolder extends RecyclerView.ViewHolder {

        TextView dateFirstCreated;
        Button button;


        public ViewHolder(View itemView) {
            super(itemView);
            dateFirstCreated = (TextView) itemView.findViewById(R.id.date_of_creation_text);
            button = (Button) itemView.findViewById(R.id.delete_btn);
        }
    }

}