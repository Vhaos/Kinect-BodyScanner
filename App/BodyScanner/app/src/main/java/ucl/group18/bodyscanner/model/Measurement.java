package ucl.group18.bodyscanner.model;

import android.os.Parcel;
import android.os.Parcelable;

/**
 * Class for all the Measurable Parameters of the Body
 * Created by Shubham on 12/04/2015.
 */
public class Measurement implements Parcelable {

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

    public    double getInsideLeg() {
        return insideLeg;
    }

    public String toString() {
        return "height: " + height + ", " +
                "hip: " + hip + ", " +
                "chest: " + chest + ", " +
                "waist: " + waist + ", " +
                "insideLeg: " + insideLeg + ", ";
    }


    protected Measurement(Parcel in) {
        height = in.readDouble();
        hip = in.readDouble();
        chest = in.readDouble();
        waist = in.readDouble();
        insideLeg = in.readDouble();
    }

    @Override
    public int describeContents() {
        return 0;
    }

    @Override
    public void writeToParcel(Parcel dest, int flags) {
        dest.writeDouble(height);
        dest.writeDouble(hip);
        dest.writeDouble(chest);
        dest.writeDouble(waist);
        dest.writeDouble(insideLeg);
    }

    @SuppressWarnings("unused")
    public static final Parcelable.Creator<Measurement> CREATOR = new Parcelable.Creator<Measurement>() {
        @Override
        public Measurement createFromParcel(Parcel in) {
            return new Measurement(in);
        }

        @Override
        public Measurement[] newArray(int size) {
            return new Measurement[size];
        }
    };
}