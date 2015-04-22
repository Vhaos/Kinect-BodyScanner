package ucl.group18.bodyscanner.database;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;

/**
 * Opens (if does not exist then Creates) the SQLite Database.
 *
 * Database Name: measurementRequests.db
 * Table Name: measurement_requests
 *
 * Columns:
 *  - _id (Primary Key/ Auto Increment)
 *  - request_id (Text) - The request ID for server
 *  - gender (Text)
 *  - processed (Text) - If the scan has been processed by the server
 *  - last_update (Text) - Last update from the server on the measurement request
 *  - height (Text) - in centimeters
 *  - hip (Text) - in centimeters
 *  - chest (Text) - in centimeters
 *  - waist (Text) - in centimeters
 *  - inside_leg (Text) - in centimeters
 *
 * Created by Shubham on 12/04/2015.
 */
public class SQLiteHelper extends SQLiteOpenHelper {

    public static final String TABLE_NAME = "measurement_requests";

    public static final String ID_COLUMN = "_id";
    public static final String REQUEST_ID_COLUMN = "request_id";
    public static final String GENDER_COLUMN = "gender";
    public static final String PROCESSED_COLUMN = "processed";
    public static final String LAST_UPDATE_COLUMN = "last_update";
    public static final String NO_OF_REQUESTS_COLUMN = "no_of_requests";
    public static final String HEIGHT_COLUMN = "height";
    public static final String HIP_COLUMN = "hip";
    public static final String CHEST_COLUMN = "chest";
    public static final String WAIST_COLUMN = "waist";
    public static final String INSIDE_LEG_COLUMN = "inside_leg";

    private static final String DATABASE_NAME = "measurementRequests.db";

    private static final int DATABASE_VERSION = 1;


    // Database creation sql statement
    private static final String DATABASE_CREATE = "create table "
            + TABLE_NAME + "("
            + ID_COLUMN + " integer primary key autoincrement, "
            + REQUEST_ID_COLUMN + " text not null,"
            + GENDER_COLUMN + " text not null,"
            + PROCESSED_COLUMN + " text not null,"
            + LAST_UPDATE_COLUMN + " text not null,"
            + NO_OF_REQUESTS_COLUMN + " integer not null,"
            + HEIGHT_COLUMN + " text null,"
            + HIP_COLUMN + " text null,"
            + CHEST_COLUMN + " text null,"
            + WAIST_COLUMN + " text null,"
            + INSIDE_LEG_COLUMN + " text null"
            + ");";

    public SQLiteHelper(Context context) {
        super(context, DATABASE_NAME, null, DATABASE_VERSION);
    }

    @Override
    public void onCreate(SQLiteDatabase db) {
        db.execSQL(DATABASE_CREATE);
    }

    @Override
    public void onUpgrade(SQLiteDatabase db, int oldVersion, int newVersion) {
        Log.v(SQLiteHelper.class.getName(),
                "Upgrading database from version " + oldVersion + " to " + newVersion);
        db.execSQL("DROP TABLE IF EXISTS " + TABLE_NAME);
        onCreate(db);
    }
}
