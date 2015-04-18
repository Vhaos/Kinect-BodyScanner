package ucl.group18.bodyscanner.model;

/**
 * Class for all the Measurable Parameters of the Body
 * Created by Shubham on 12/04/2015.
 */
public class Measurement {

    double height;
    double hip;
    double chest;
    double waist;
    double insideLeg;


    public Measurement (){ }

    public void setMeasurements( double height, double hip, double chest,
                                 double waist, double insideLeg){

        this.height = height;
        this.hip = hip;
        this.chest = chest;
        this.waist = waist;
        this.insideLeg = insideLeg;

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

    public String toString() {
        return "height: " + height + ", " +
               "hip: " + hip + ", " +
               "chest: " + chest + ", " +
               "waist: " + waist + ", " +
               "insideLeg: " + insideLeg + ", ";
    }

}
