package ucl.group18.bodyscanner;

import android.content.Context;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.text.SimpleDateFormat;
import java.util.Date;

/**
 * Created by Shubham on 25/04/2015.
 */
public class Logger {

    private static final String FILE_NAME = "log.txt";
    Context context;

    public Logger (Context context){
        this.context = context;
    }

    public void write(String string){

        String existingLog = readFullLog();

        try {
            FileOutputStream fileout = context.openFileOutput(FILE_NAME, Context.MODE_PRIVATE);
            OutputStreamWriter outputWriter=new OutputStreamWriter(fileout);

            SimpleDateFormat dateFormat = new SimpleDateFormat("dd-MM-yyyy HH:mm:ss");
            String date = dateFormat.format(new Date());

            outputWriter.write( existingLog + "[" + date + "]" + "\n" + string + "\n"+ "\n");
            outputWriter.close();

        } catch (Exception e) {
            e.printStackTrace();
        }


    }

    public String readFullLog(){

        String log = "";

        try {
            FileInputStream fileIn= context.openFileInput(FILE_NAME);
            InputStreamReader InputRead= new InputStreamReader(fileIn);

            char[] inputBuffer= new char[100];
            String s="";
            int charRead;

            while ((charRead=InputRead.read(inputBuffer))>0) {
                // char to string conversion
                String readstring=String.copyValueOf(inputBuffer,0,charRead);
                s +=readstring;
            }

            InputRead.close();

            log = s;


        } catch (Exception e) {
            e.printStackTrace();
            return "";
        }

        return log;
    }


}
