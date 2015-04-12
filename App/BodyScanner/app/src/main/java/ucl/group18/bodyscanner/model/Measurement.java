package ucl.group18.bodyscanner.model;

/**
 * Created by Shubham on 12/04/2015.
 */
public class Measurement {

    int id;
    double height;
    double hip;
    double chest;
    double waist;
    double insideLeg;
    boolean processed = false;


    public Measurement (int id){
        this.id = id;
    }

    public void setMeasurements( double height, double hip, double chest,
                                 double waist, double insideLeg){

        this.height = id;
        this.hip = id;
        this.chest = id;
        this.waist = id;
        this.insideLeg = id;

    }

    public int getId() {
        return id;
    }

    public double getHeight() {
        return height;
    }

    public double getHip() {
        return hip;
    }

    public double getChest() {
        return chest;
    }

    public double getWaist() {
        return waist;
    }

    public double getInsideLeg() {
        return insideLeg;
    }

    public boolean isProcessed() {
        return processed;
    }
}
