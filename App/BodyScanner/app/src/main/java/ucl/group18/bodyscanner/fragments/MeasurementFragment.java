package ucl.group18.bodyscanner.fragments;

import android.app.Activity;
import android.content.Context;
import android.net.Uri;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.text.Layout;
import android.util.Log;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.TextView;

import ucl.group18.bodyscanner.Logger;
import ucl.group18.bodyscanner.R;
import ucl.group18.bodyscanner.SharedPrefsHandler;
import ucl.group18.bodyscanner.UnitConverter;
import ucl.group18.bodyscanner.model.Measurement;
import ucl.group18.bodyscanner.model.MeasurementRequest;

/**
 * This is a fragment which displays the measurements.
 * A simple {@link Fragment} subclass.
 * Activities that contain this fragment must implement the
 * {@link MeasurementFragment.OnFragmentInteractionListener} interface
 * to handle interaction events.
 * Use the {@link MeasurementFragment#newInstance} factory method to
 * create an instance of this fragment.
 */
public class MeasurementFragment extends Fragment {
    private static final String MEASUREMENT= "measurement";
    private static final String LOG_TAG = "MeasurementFragment";

    private Measurement measurement;
    private OnFragmentInteractionListener mListener;

    String heightInPreferredUnits;
    String hipInPreferredUnits;
    String chestInPreferredUnits;
    String waistInPreferredUnits;
    String insideLegInPreferredUnits;

    TextView height_tv;
    TextView hip_tv;
    TextView chest_tv;
    TextView waist_tv;
    TextView inside_leg_tv;

    /**
     * Use this factory method to create a new instance of
     * this fragment using the provided parameters.
     * @param measurement The measurements that are  needed to display
     * @return A new instance of fragment MeasurementFragment.
     */
    // TODO: Rename and change types and number of parameters
    public static MeasurementFragment newInstance( Measurement measurement) {
        MeasurementFragment fragment = new MeasurementFragment();
        Bundle args = new Bundle();
        args.putParcelable(MEASUREMENT, measurement);
        fragment.setArguments(args);
        return fragment;
    }

    public MeasurementFragment() {
        // Required empty public constructor
    }

    @Override
    public void onResume(){
        super.onResume();
        Log.v(LOG_TAG,"onResume");
        populateWithPreferredUnits(measurement);
        setTextViewsWithValues();
    }

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        if (getArguments() != null) {
            measurement = getArguments().getParcelable(MEASUREMENT);
        }
    }

    private void populateWithPreferredUnits(Measurement measurement){

        double heightInCms = measurement.getHeight();
        double hipInCms = measurement.getHip();
        double chestInCms = measurement.getChest();
        double waistInCms = measurement.getWaist();
        double insideLegInCms = measurement.getInsideLeg();

        new Logger(getActivity()).write("Fragment: " + measurement.toString());

        SharedPrefsHandler prefs = new SharedPrefsHandler(getActivity());
        String preferredUnits = prefs.getPreferredUnits();

        Log.v(LOG_TAG, preferredUnits);

        if (preferredUnits.equals("cm")){

            heightInPreferredUnits = roundTo1dp(heightInCms) + " cm";
            hipInPreferredUnits = roundTo1dp(hipInCms) + " cm";
            chestInPreferredUnits = roundTo1dp(chestInCms) + " cm";
            waistInPreferredUnits = roundTo1dp(waistInCms) + " cm";
            insideLegInPreferredUnits = roundTo1dp(insideLegInCms)+ " cm";


        }else if (preferredUnits.equals("inch")){

            heightInPreferredUnits = Math.round(UnitConverter.cmToInch(heightInCms)) + " inch";
            hipInPreferredUnits = Math.round(UnitConverter.cmToInch(hipInCms)) + " inch";
            chestInPreferredUnits = Math.round(UnitConverter.cmToInch(chestInCms)) + " inch";
            waistInPreferredUnits = Math.round(UnitConverter.cmToInch(waistInCms)) + " inch";
            insideLegInPreferredUnits = Math.round(UnitConverter.cmToInch(insideLegInCms)) + " inch";

        }else{
            heightInPreferredUnits = UnitConverter.cmToDefault(heightInCms, UnitConverter.BodyPart.HEIGHT);
            hipInPreferredUnits = UnitConverter.cmToDefault(hipInCms, UnitConverter.BodyPart.HIP);
            chestInPreferredUnits = UnitConverter.cmToDefault(chestInCms, UnitConverter.BodyPart.CHEST);
            waistInPreferredUnits = UnitConverter.cmToDefault(waistInCms, UnitConverter.BodyPart.WAIST);
            insideLegInPreferredUnits = UnitConverter.cmToDefault(insideLegInCms, UnitConverter.BodyPart.INSIDE_LEG);
        }
    }

    @Override
    public View onCreateView(LayoutInflater inflater, ViewGroup container,
                             Bundle savedInstanceState) {

        populateWithPreferredUnits(measurement);

        View layout = inflater.inflate(R.layout.fragment_measurement, container, false);

        height_tv = (TextView)layout.findViewById(R.id.height_value_text);
        hip_tv = (TextView)layout.findViewById(R.id.hip_value_text);
        chest_tv = (TextView)layout.findViewById(R.id.chest_bust_value_text);
        waist_tv = (TextView)layout.findViewById(R.id.waist_value_text);
        inside_leg_tv = (TextView)layout.findViewById(R.id.inside_leg_value_text);



        setTextViewsWithValues();

        return layout;
    }

    private void setTextViewsWithValues() {

        height_tv.setText(heightInPreferredUnits);
        hip_tv.setText(hipInPreferredUnits);
        chest_tv.setText(chestInPreferredUnits);
        waist_tv.setText(waistInPreferredUnits);
        inside_leg_tv.setText(insideLegInPreferredUnits);

    }


    @Override
    public void onAttach(Activity activity) {
        super.onAttach(activity);
        try {
            mListener = (OnFragmentInteractionListener) activity;
        } catch (ClassCastException e) {
            throw new ClassCastException(activity.toString()
                    + " must implement OnFragmentInteractionListener");
        }
    }

    private double roundTo1dp(double number){
        return (Math.round(number * 10) / 10.0);
    }

    @Override
    public void onDetach() {
        super.onDetach();
        mListener = null;
    }

    /**
     * This interface must be implemented by activities that contain this
     * fragment to allow an interaction in this fragment to be communicated
     * to the activity and potentially other fragments contained in that
     * activity.
     * <p/>
     * See the Android Training lesson <a href=
     * "http://developer.android.com/training/basics/fragments/communicating.html"
     * >Communicating with Other Fragments</a> for more information.
     */
    public interface OnFragmentInteractionListener {
        public void onFragmentInteraction(Uri uri);
    }

}
