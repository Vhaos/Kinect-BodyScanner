package ucl.group18.bodyscanner;

import android.content.Intent;
import android.os.Bundle;
import android.preference.PreferenceActivity;
import android.preference.PreferenceFragment;
import android.preference.PreferenceManager;
import android.support.v7.app.ActionBar;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.widget.LinearLayout;

import ucl.group18.bodyscanner.R;

/**
 * Created by Shubham on 16/03/2015.
 */
public class SettingsActivity extends PreferenceActivity {

    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);


        /*
        Instantiating the Settings Fragment and setting it to the main content
         */
        PreferenceManager.setDefaultValues(this,
                R.xml.preference, false);

        // Load the preferences from an XML resource
        addPreferencesFromResource(R.xml.preference);


    }

    @Override
    protected void onPostCreate(Bundle savedInstanceState) {
        super.onPostCreate(savedInstanceState);

        /*
        This is a hack to show the ActionBar in SettingsActivity.
        This because of a bug in AppCompat v21 library which
        for some reason removed the action bar in PreferenceActivity.

        Essentially, this draws a toolbar at the top of the screen.
        kudos to stackoverflow.
         */
        LinearLayout root = (LinearLayout) findViewById(android.R.id.list).getParent().getParent().getParent();
        Toolbar bar = (Toolbar) LayoutInflater.from(this).inflate(R.layout.preferences_toolbar, root, false);
        root.addView(bar, 0); // insert at top
        bar.setNavigationOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });
    }

    public void onHistoryClick (View view){

        Intent intent = new Intent();

    }


}
