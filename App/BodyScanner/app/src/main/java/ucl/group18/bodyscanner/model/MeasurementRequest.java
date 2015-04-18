package ucl.group18.bodyscanner.model;

import android.util.Log;

import java.util.Calendar;

/**
 * Class for a measurement request to the server
 * Has parity with a measurement request record in the MeasurementRequests Database.
 * Holds information on:
 * 1. Database Unique ID
 * 2. server request ID
 * 3. if scan processed by server
 * 4. time of last request to the server
 * 5. Measurements (if scan is processed)
 *
 * Created by Shubham on 12/04/2015.
 */
public class MeasurementRequest {

    private static final String LOG_TAG = "MeasurementRequest";
    long id = -1;
    String requestID;
    boolean processed = false;
    Calendar lastRequest;
    Measurement measurement = null;

    public MeasurementRequest(String requestID){
        this.requestID = requestID;
    }

    public Measurement getMeasurements (){

        if (processed){
            return measurement;
        }else{
            Log.e(LOG_TAG,"Measurement Request not processed by Server yet");
            return null;
        }

    }

    public long getId() {
        return id;
    }

    public String getRequestID() {
        return requestID;
    }

    public void setId(long id) { this.id=id; }

    public boolean isProcessed() {
        return processed;
    }

    public void setProcessed(boolean processed) {
        this.processed = processed;
    }

    public Calendar getLastRequest() {
        return lastRequest;
    }

    public void setLastRequest(Calendar lastRequest) {
        this.lastRequest = lastRequest;
    }

    public void setMeasurement(Measurement measurement) {


        this.measurement = measurement;
        Log.v("MR", measurement.toString());
        Log.v("MR", this.measurement.toString());
    }
}
