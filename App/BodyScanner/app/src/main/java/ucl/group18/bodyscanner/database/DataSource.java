package ucl.group18.bodyscanner.database;

import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.SQLException;
import android.database.sqlite.SQLiteDatabase;

import java.util.ArrayList;
import java.util.List;

import ucl.group18.bodyscanner.model.MeasurementRequest;

/**
 * Created by Shubham on 12/04/2015.
 */
public class DataSource {

    // Database fields
    private SQLiteDatabase database;
    private SQLiteHelper dbHelper;
    private String[] allColumns = {  };

    public DataSource(Context context) {
        dbHelper = new SQLiteHelper(context);
    }

    public void open() throws SQLException {
        database = dbHelper.getWritableDatabase();
    }

    public void close() {
        dbHelper.close();
    }
    /*
    public MeasurementRequest addMeasurementRequest(MeasurementRequest measurementRequest) {

        ContentValues values = new ContentValues();
        values.put(SQLiteHelper.COLUMN_MeasurementRequest, MeasurementRequest);
        long insertId = database.insert(SQLiteHelper.TABLE_MeasurementRequestS, null,
                values);
        Cursor cursor = database.query(SQLiteHelper.TABLE_MeasurementRequestS,
                allColumns, SQLiteHelper.COLUMN_ID + " = " + insertId, null,
                null, null, null);
        cursor.moveToFirst();
        MeasurementRequest newMeasurementRequest = cursorToMeasurementRequest(cursor);
        cursor.close();
        return newMeasurementRequest;

    }

    public void deleteMeasurementRequest(MeasurementRequest MeasurementRequest) {
        long id = MeasurementRequest.getId();
        System.out.println("MeasurementRequest deleted with id: " + id);
        database.delete(SQLiteHelper.TABLE_MeasurementRequestS, SQLiteHelper.COLUMN_ID
                + " = " + id, null);
    }

    public List<MeasurementRequest> getAllMeasurementRequests() {
        List<MeasurementRequest> MeasurementRequests = new ArrayList<MeasurementRequest>();

        Cursor cursor = database.query(SQLiteHelper.TABLE_MeasurementRequestS,
                allColumns, null, null, null, null, null);

        cursor.moveToFirst();
        while (!cursor.isAfterLast()) {
            MeasurementRequest measurementRequest = cursorToMeasurementRequest(cursor);
            MeasurementRequests.add(measurementRequest);
            cursor.moveToNext();
        }
        // make sure to close the cursor
        cursor.close();
        return MeasurementRequests;
    }

    private MeasurementRequest cursorToMeasurementRequest(Cursor cursor) {
        MeasurementRequest measurementRequest = new MeasurementRequest();
        measurementRequest.setId(cursor.getLong(0));
        measurementRequest.setMeasurementRequest(cursor.getString(1));
        return measurementRequest;
    }
    */
}
