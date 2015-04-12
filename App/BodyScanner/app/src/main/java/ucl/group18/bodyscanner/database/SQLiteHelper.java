package ucl.group18.bodyscanner.database;

import android.content.Context;
import android.database.sqlite.SQLiteDatabase;
import android.database.sqlite.SQLiteOpenHelper;
import android.util.Log;

/**
 * Created by Shubham on 12/04/2015.
 */
public class SQLiteHelper extends SQLiteOpenHelper {

    public static final String TABLE_NAME = "comments";

    public static final String ID_COLUMN = "_id";
    public static final String PROCESSED_COLUMN = "processed";
    public static final String LAST_UPDATE_COLUMN = "last_update";
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
            + PROCESSED_COLUMN + " text not null"
            + LAST_UPDATE_COLUMN + " text not null"
            + HEIGHT_COLUMN + " text not null"
            + HIP_COLUMN + " text not null"
            + CHEST_COLUMN + " text not null"
            + WAIST_COLUMN + " text not null"
            + INSIDE_LEG_COLUMN + " text not null"
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
