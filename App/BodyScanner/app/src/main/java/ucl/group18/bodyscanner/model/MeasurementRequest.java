package ucl.group18.bodyscanner.model;

import java.util.Calendar;

/**
 * Created by Shubham on 12/04/2015.
 */
public class MeasurementRequest {

    int id;
    boolean processed = false;
    Calendar lastRequest;
    Measurement measurement;

    public MeasurementRequest(int id){
        this.id = id;
    }

    public Measurement getMeasurements (){

        if (processed){
            return measurement;
        }else{
            return null;
        }

    }

    public int getId() {
        return id;
    }

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

    public Measurement getMeasurement() {
        return measurement;
    }

    public void setMeasurement(Measurement measurement) {
        this.measurement = measurement;
    }
}
