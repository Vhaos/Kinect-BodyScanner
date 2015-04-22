package ucl.group18.bodyscanner;

/**
 * Static Class to convert unis
 * Created by Shubham on 22/04/2015.
 */
public class UnitConverter {

    public static enum BodyPart{HEIGHT,CHEST,HIP, WAIST,INSIDE_LEG}

    private static final double ONE_FOOT_TO_CMS = 30.48;
    private static final double ONE_INCH_TO_CMS = 2.54;

    /**
     * Converts measurement (in cm) to appropriate string representation
     * @param measurementInCms
     * @param bodyPart The bodyPart being converted
     * @return String representation of the measurement (can be inch, cm, feet etc.)
     */
    public static String cmToDefault(double measurementInCms, BodyPart bodyPart){

        String result;

        switch (bodyPart){
            case HEIGHT:
                int foot = (int) (measurementInCms/ONE_FOOT_TO_CMS);
                double leftOverCms = measurementInCms - (foot * ONE_FOOT_TO_CMS);
                int inches = (int) Math.round(cmToInch(leftOverCms));
                result = foot + "\' " + inches + "\"";
                break;

            default:
                result = String.valueOf ((int) cmToInch(measurementInCms)) + " inch";
                break;
        }

        return result;

    }

    public static double cmToInch(double measurementInCms){
        return (measurementInCms/ONE_INCH_TO_CMS);
    }

}
