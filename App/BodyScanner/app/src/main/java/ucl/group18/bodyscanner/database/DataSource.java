package ucl.group18.bodyscanner.database;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;
import android.util.Log;

import java.text.DateFormat;
import java.text.ParseException;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Calendar;
import java.util.List;
import java.util.Locale;

import ucl.group18.bodyscanner.model.Measurement;
import ucl.group18.bodyscanner.model.MeasurementRequest;

/**
 * This is a Data Access Object for the Measurement Request Database
 * Created by Shubham on 12/04/2015.
 */
public class DataSource {

    String LOG_TAG = "MeasurementDataSource";

    // Database fields
    private SQLiteDatabase database;
    private SQLiteHelper dbHelper;
    private String[] allColumns = { SQLiteHelper.ID_COLUMN,
                                    SQLiteHelper.REQUEST_ID_COLUMN,
                                    SQLiteHelper.GENDER_COLUMN,
                                    SQLiteHelper.PROCESSED_COLUMN,
                                    SQLiteHelper.LAST_UPDATE_COLUMN,
                                    SQLiteHelper.FIRST_CREATED_COLUMN,
                                    SQLiteHelper.NO_OF_REQUESTS_COLUMN,
                                    SQLiteHelper.HEIGHT_COLUMN,
                                    SQLiteHelper.HIP_COLUMN,
                                    SQLiteHelper.CHEST_COLUMN,
                                    SQLiteHelper.WAIST_COLUMN,
                                    SQLiteHelper.INSIDE_LEG_COLUMN,
                                    };

    SimpleDateFormat dateFormat = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");

    /**
     * Constructs a new DataSource Object
     * @param context Context of the Activity
     */
    public DataSource(Context context) {
        dbHelper = new SQLiteHelper(context);
    }

    /**
     * Opens the SQLite Database for querying/modifying
     * Remember to close the database after accessing
     * @throws SQLException
     */
    private void open() throws SQLException {
        database = dbHelper.getWritableDatabase();
    }

    /**
     * Closes the Database
     */
    private void close() {
        dbHelper.close();
    }

    /**
     * Inserts a new measurement Request to the Database
     * @param measurementRequest The MeasurementRequest to be inserted
     * @return false if insertion failed true otherwise
     */
    public boolean addMeasurementRequest(MeasurementRequest measurementRequest) {

        Log.v(LOG_TAG, "Adding Measurement");
        open();
        ContentValues values = new ContentValues();
        values.put(SQLiteHelper.REQUEST_ID_COLUMN, measurementRequest.getRequestID());
        values.put(SQLiteHelper.GENDER_COLUMN, measurementRequest.getGender().name());
        values.put(SQLiteHelper.PROCESSED_COLUMN, Boolean.toString(measurementRequest.isProcessed()));
        values.put(SQLiteHelper.LAST_UPDATE_COLUMN, dateFormat.format(
                measurementRequest.getLastRequest().getTime()));
        values.put(SQLiteHelper.FIRST_CREATED_COLUMN, dateFormat.format(
                measurementRequest.getFirstCreated().getTime()));
        values.put(SQLiteHelper.NO_OF_REQUESTS_COLUMN,measurementRequest.getNoOfRequests());

        long insertId = database.insert(SQLiteHelper.TABLE_NAME, null, values);

        if (insertId == -1){
            Log.e(LOG_TAG, "Could not add record");
            return false;
        }

        Log.v(LOG_TAG, "Measurement Added with ID: " + insertId);
        measurementRequest.setId(insertId);
        close();
        return true;

    }

    /**
     * Deletes a Measurement Request Record
     * The Measurement Request must have an ID otherwise nothing is deleted
     * @param measurementRequest  The MeasurementRequest to be deleted
     */
    public void deleteMeasurementRequest(MeasurementRequest measurementRequest) {
        long id = measurementRequest.getId();
        deleteMeasurementRequest(id);

    }

    /**
     * Deletes a Measurement Request Record
     * @param id  The MeasurementRequest to be deleted
     */
    public void deleteMeasurementRequest(long id) {
        open();
        int affectedRows = database.delete(SQLiteHelper.TABLE_NAME, SQLiteHelper.ID_COLUMN + " = " + id, null);

        if (affectedRows > 0){
            Log.v(LOG_TAG, "MeasurementRequest deleted with id: " + id);
        }else{
            Log.e(LOG_TAG, "Attempt to delete (ID:  " + id + ") FAILED, Nothing deleted");
        }
        close();

    }

    /**
     * Returns a list of all measurement requests
     * @return List of MeasurementRequests
     */
    public List<MeasurementRequest> getAllMeasurementRequests() {
        open();
        Log.v(LOG_TAG, "Getting All Measurements");
        List<MeasurementRequest> measurementRequests = new ArrayList<MeasurementRequest>();

        Cursor cursor = database.query(SQLiteHelper.TABLE_NAME,
                allColumns, null, null, null, null, null);
        Log.v(LOG_TAG, "Queried");

        cursor.moveToFirst();

        if (cursor.getCount() <= 0){ // Return null if no resulting query
            Log.e(LOG_TAG, "Nothing in the Table");
            return null;
        }
        while (!cursor.isAfterLast()) {
            MeasurementRequest measurementRequest = cursorToMeasurementRequest(cursor);
            measurementRequests.add(measurementRequest);
            cursor.moveToNext();
        }
        cursor.close();
        close();
        return measurementRequests;

    }

    /**
     * Returns a list of all measurement requests
     * @return List of MeasurementRequests
     */
    public List<MeasurementRequest> getAllUnProcessedMeasurementRequests() {
        open();
        List<MeasurementRequest> MeasurementRequests = new ArrayList<MeasurementRequest>();

        Cursor cursor = database.query(SQLiteHelper.TABLE_NAME,
                allColumns, SQLiteHelper.PROCESSED_COLUMN +"=?" , new String[]{Boolean.toString(false)}, null, null, null);

        cursor.moveToFirst();

        if (cursor.getCount() <= 0){ // Return null if no resulting query
            return null;
        }

        while (!cursor.isAfterLast()) {
            MeasurementRequest measurementRequest = cursorToMeasurementRequest(cursor);
            MeasurementRequests.add(measurementRequest);
            cursor.moveToNext();
        }

        cursor.close();
        close();
        return MeasurementRequests;
    }

    /**
     * Returns the processed latest measurement request
     * @return latest processed measurement request, null if none exist
     */
    public MeasurementRequest getLatestProcessedMeasurementRequests() {
        open();
        List<MeasurementRequest> MeasurementRequests = new ArrayList<MeasurementRequest>();

        Cursor cursor = database.query(SQLiteHelper.TABLE_NAME,
                allColumns, SQLiteHelper.PROCESSED_COLUMN +"=?" , new String[]{Boolean.toString(true)}, null,
                null, SQLiteHelper.FIRST_CREATED_COLUMN + " DESC", null);

        Log.v(LOG_TAG, "Processed MRs : " + cursor.getCount());

        cursor.moveToFirst();

        if (cursor.getCount() <= 0){ // Return null if no resulting query
            return null;
        }

        MeasurementRequest measurementRequest = cursorToMeasurementRequest(cursor);
        cursor.close();
        close();
        return measurementRequest;

    }

    /**
     * Updates a measurement request record
     * @param measurementRequest the measurement request to be updated
     * @return false if update failed true otherwise
     */
    public boolean updateMeasurementRequest(MeasurementRequest measurementRequest){
        open();
        long id = measurementRequest.getId();

        if (id == -1){
            Log.e(LOG_TAG, "Couldn't update record, No Id found in measurementRequest");
            return false;
        }

        ContentValues values = new ContentValues();
        values.put(SQLiteHelper.ID_COLUMN, id);
        values.put(SQLiteHelper.REQUEST_ID_COLUMN, measurementRequest.getRequestID());
        values.put(SQLiteHelper.GENDER_COLUMN, measurementRequest.getGender().name());
        values.put(SQLiteHelper.PROCESSED_COLUMN, Boolean.toString(measurementRequest.isProcessed()));
        values.put(SQLiteHelper.LAST_UPDATE_COLUMN, dateFormat.format(
                measurementRequest.getLastRequest().getTime()));
        values.put(SQLiteHelper.FIRST_CREATED_COLUMN, dateFormat.format(
                measurementRequest.getFirstCreated().getTime()));
        values.put(SQLiteHelper.NO_OF_REQUESTS_COLUMN, measurementRequest.getNoOfRequests());

        if (measurementRequest.getMeasurements() != null){

            Measurement measurement = measurementRequest.getMeasurements();

            values.put(SQLiteHelper.HEIGHT_COLUMN, Double.toString( measurement.getHeight()));
            values.put(SQLiteHelper.CHEST_COLUMN, Double.toString( measurement.getChest()));
            values.put(SQLiteHelper.WAIST_COLUMN, Double.toString( measurement.getWaist()));
            values.put(SQLiteHelper.HIP_COLUMN, Double.toString( measurement.getHip()));
            values.put(SQLiteHelper.INSIDE_LEG_COLUMN, Double.toString( measurement.getInsideLeg()));
        }

        int affectedRows = database.update(SQLiteHelper.TABLE_NAME, values, SQLiteHelper.ID_COLUMN + "=" + id, null);

        if (affectedRows == 0){
            Log.e(LOG_TAG, "No rows updated");
            return false;
        }
        close();
        return true;
    }

    /**
     * Converts a cursor to MeasurementRequest Object.
     * @param cursor cursor to be converted
     * @return converted MeasurementRequest Object
     */
    private MeasurementRequest cursorToMeasurementRequest(Cursor cursor) {

        int id = cursor.getInt(cursor.getColumnIndex(SQLiteHelper.ID_COLUMN));
        String requestId = cursor.getString(cursor.getColumnIndex(SQLiteHelper.REQUEST_ID_COLUMN));
       MeasurementRequest.Gender gender = MeasurementRequest.Gender.valueOf(
               cursor.getString(cursor.getColumnIndex(SQLiteHelper.GENDER_COLUMN)).toUpperCase(Locale.US));
        String lastUpdateString = cursor.getString(cursor.getColumnIndex(SQLiteHelper.LAST_UPDATE_COLUMN));
        String firstCreatedString = cursor.getString(cursor.getColumnIndex(SQLiteHelper.FIRST_CREATED_COLUMN));
        String processed = cursor.getString(cursor.getColumnIndex(SQLiteHelper.PROCESSED_COLUMN));
        int noOfRequest = cursor.getInt(cursor.getColumnIndex(SQLiteHelper.NO_OF_REQUESTS_COLUMN));

        MeasurementRequest measurementRequest = new MeasurementRequest(requestId,gender);

        Calendar lastUpdate = Calendar.getInstance();
        try {
            lastUpdate.setTime(dateFormat.parse(lastUpdateString));
        } catch (ParseException e) {
            e.printStackTrace();
            Log.e(LOG_TAG,"Couldn't Parse: + " + lastUpdateString);
        }

        Calendar firstCreated = Calendar.getInstance();
        try {
            firstCreated.setTime(dateFormat.parse(firstCreatedString));
        } catch (ParseException e) {
            e.printStackTrace();
            Log.e(LOG_TAG,"Couldn't Parse: + " + firstCreatedString);
        }

        measurementRequest.setId(id);
        measurementRequest.setLastRequest(lastUpdate);
        measurementRequest.setFirstCreated(firstCreated);
        measurementRequest.setProcessed(Boolean.parseBoolean(processed));
        measurementRequest.setNoOfRequests(noOfRequest);

        /*
        Getting the Measurements if measurement exists
        */
        if (!cursor.isNull(cursor.getColumnIndex(SQLiteHelper.HEIGHT_COLUMN))) {

            String height = cursor.getString(cursor.getColumnIndex(SQLiteHelper.HEIGHT_COLUMN));
            String chest = cursor.getString(cursor.getColumnIndex(SQLiteHelper.CHEST_COLUMN));
            String waist = cursor.getString(cursor.getColumnIndex(SQLiteHelper.WAIST_COLUMN));
            String hip = cursor.getString(cursor.getColumnIndex(SQLiteHelper.HIP_COLUMN));
            String inside_leg = cursor.getString(cursor.getColumnIndex(SQLiteHelper.INSIDE_LEG_COLUMN));

            Measurement measurements = new Measurement();

            measurements.setMeasurements(Double.valueOf(height),
                    Double.valueOf(chest),
                    Double.valueOf(waist),
                    Double.valueOf(hip),
                    Double.valueOf(inside_leg));

            measurementRequest.setMeasurement(measurements);
        }

        return measurementRequest;
    }
}
