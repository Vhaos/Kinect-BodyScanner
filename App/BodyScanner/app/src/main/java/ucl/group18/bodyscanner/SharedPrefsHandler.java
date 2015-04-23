package ucl.group18.bodyscanner;

import android.content.Context;
import android.content.SharedPreferences;
import android.preference.PreferenceManager;

import java.util.Map;
import java.util.Set;

/**
 * Created by Shubham on 22/04/2015.
 */
public class SharedPrefsHandler {

    SharedPreferences prefs;

    public SharedPrefsHandler(Context context){

        prefs = PreferenceManager.getDefaultSharedPreferences(context);
    }

    public String getPreferredUnits(){

        return prefs.getString("measurement_units", "default");

    }

    public Boolean isIncognitoMode(){

        return prefs.getBoolean("incognito_mode", false);

    }

    public String getServerName(){

        return prefs.getString("server_name_ip","localhost");

    }

}
