package ucl.group18.bodyscanner;

import android.content.Context;
import android.os.AsyncTask;
import android.util.Log;

import org.apache.http.HttpResponse;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.URI;
import java.net.URISyntaxException;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import ucl.group18.bodyscanner.model.Measurement;
import ucl.group18.bodyscanner.model.MeasurementRequest;

/**
 * Created by Shubham on 18/04/2015.
 */
public class ServerConnect  {

    private static final String LOG_TAG = "Network";
    Context context;

    public interface ServerConnectCallback{
        public void getMeasurementCallback(Measurement measurement);
    }

    private String ip_name = "shubhampc";

    public String checkURL = "http://" + ip_name + "/server/check/"; // + request_Id

    public String negativeResult = "OK, Scan not yet finished";
    //5531bd226edd8 - unfinished
    //5522e70fe6cb4 - finished
    //public interface {}

    public ServerConnect(Context context){
        this.context = context;
    }

    public void getMeasurementsFromServerAsync(MeasurementRequest measurementRequest,
                                               ServerConnectCallback callback){

        new AsyncTask<Object,Void,Boolean>(){

            @Override
            protected Boolean doInBackground(Object... params) {

                MeasurementRequest mr = (MeasurementRequest) params[0];
                ServerConnectCallback callback = (ServerConnectCallback) params[1];

                Measurement m = getMeasurementsFromServer (mr);
                Log.v(LOG_TAG,"Calling Back...");
                callback.getMeasurementCallback(m);

                return true;
            }
        }.execute(measurementRequest,callback);

    }

    public Measurement getMeasurementsFromServer(MeasurementRequest measurementRequest){

        Measurement measurement = null;

        HttpClient client = new DefaultHttpClient();
        HttpResponse response = null;

        try {

            String url = checkURL + measurementRequest.getRequestID();
            Log.v(LOG_TAG, url);
            response = client.execute(new HttpGet(url));

            BufferedReader in = new BufferedReader(new InputStreamReader(response.getEntity()
                    .getContent()));

            String line = "";

            while ((line = in.readLine()) != null) {

                Log.v(LOG_TAG, line);
                if (line.contains(negativeResult)){
                   return null;
                }

                try {
                    JSONObject jObject = new JSONObject(line);

                    if (jObject.has("measure")) {
                        String measurements = jObject.getString("measure");
                        Log.e("measure", measurements);
                        measurement = parseMeasurements(measurements);
                    }
                }catch (JSONException e){
                    continue;
                }catch (NumberFormatException e){
                    return null;
                }


            }

        } catch (IOException e) {
            e.printStackTrace();
            return null;
       }

        return measurement;

    }

    private Measurement parseMeasurements(String measurements) throws NumberFormatException{

        Pattern p = Pattern.compile("\"(.*?)\"");
        Matcher m = p.matcher(measurements);

        ArrayList<String> measurementArray = new ArrayList<String>();

        while (m.find()) {
            measurementArray.add(m.group(1));
        }

        Measurement measurement = new Measurement();
        measurement.setMeasurements(Double.parseDouble(measurementArray.get(0)),
                                    Double.parseDouble(measurementArray.get(1)),
                                    Double.parseDouble(measurementArray.get(2)),
                                    Double.parseDouble(measurementArray.get(3)),
                                    Double.parseDouble(measurementArray.get(4)));

        return measurement;
    }

}
