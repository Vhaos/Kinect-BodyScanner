package ucl.group18.bodyscanner;

import android.app.Application;
import android.test.ApplicationTestCase;

import junit.framework.Assert;

import java.util.Calendar;
import java.util.List;

import ucl.group18.bodyscanner.cloudconnection.ServerConnect;
import ucl.group18.bodyscanner.database.DataSource;
import ucl.group18.bodyscanner.model.Measurement;
import ucl.group18.bodyscanner.model.MeasurementRequest;

/**
 * Make sure to run tests after clearing existing data on the app
 */
public class ApplicationTest extends ApplicationTestCase<Application> {
    public ApplicationTest() {
        super(Application.class);
    }

    public void testUnitConverter_cmToInch(){

        UnitConverter unitConverter = new UnitConverter();
        Double inch  = unitConverter.cmToInch(35);
        Double expected = 35.0/2.54;
        Assert.assertEquals(expected,inch);
    }

    public void testUnitConverter_cmToDefault(){

        UnitConverter unitConverter = new UnitConverter();
        String result  = unitConverter.cmToDefault(181, UnitConverter.BodyPart.HEIGHT);
        String expected = "5' 11\"";
        Assert.assertEquals(expected,result);
    }

    public void testServerConnect_parseMeasurements(){

        ServerConnect serverConnect = new ServerConnect(getContext());

        String input = " [\"147.529\", \"105.469\", \"94.8168\", \"81.6679\", \"75.0965\"]";
        Measurement measurement =  serverConnect.parseMeasurements(input);
        Assert.assertEquals(measurement.getHeight(), 147.529);
        Assert.assertEquals(measurement.getHip(), 105.469);
        Assert.assertEquals(measurement.getChest(), 94.8168);
        Assert.assertEquals(measurement.getWaist(), 81.6679);
        Assert.assertEquals(measurement.getInsideLeg(), 75.0965);

    }

    public void testDataBase_unProcessedQuery(){

        //Set up Fake Data
        MeasurementRequest mr = new MeasurementRequest("foobar", MeasurementRequest.Gender.MALE);
        mr.setFirstCreated(Calendar.getInstance());
        mr.setLastRequest(Calendar.getInstance());
        mr.setNoOfRequests(2);
        mr.setProcessed(false);


        DataSource ds= new DataSource(getContext());


        ds.addMeasurementRequest(mr);

        List<MeasurementRequest> mrList =  ds.getAllUnProcessedMeasurementRequests();

        Assert.assertEquals(mrList.size(),1); // Test if only one returned.

        MeasurementRequest testMr = mrList.get(0);

        Assert.assertEquals(testMr.getRequestID(),"foobar");
        Assert.assertEquals(testMr.getGender(),MeasurementRequest.Gender.MALE);
        Assert.assertEquals(testMr.isProcessed(),false);
        Assert.assertEquals(testMr.getNoOfRequests(),2);

    }

    public void testDatabase_saving_measurement(){

        DataSource ds= new DataSource(getContext());

        //Set up fake data
        Measurement measurement = new Measurement();
        measurement.setMeasurements(181,32,45,67,78);


        MeasurementRequest mr = new MeasurementRequest("hello", MeasurementRequest.Gender.MALE);
        mr.setFirstCreated(Calendar.getInstance());
        mr.setLastRequest(Calendar.getInstance());
        mr.setNoOfRequests(2);
        mr.setProcessed(false);
        mr.setMeasurement(measurement);

        ds.addMeasurementRequest(mr);
        ds.updateMeasurementRequest(mr);//udating to include measurements aswell

        List<MeasurementRequest> mrList = ds.getAllMeasurementRequests();

        for (MeasurementRequest testMr : mrList){

            if (testMr.getId() == mr.getId()){
                Measurement measurement1 = testMr.getMeasurements();
                Assert.assertEquals(measurement.getHeight(), 181.0);
                Assert.assertEquals(measurement.getHip(), 32.0);
                Assert.assertEquals(measurement.getChest(), 45.0);
                Assert.assertEquals(measurement.getWaist(), 67.0);
                Assert.assertEquals(measurement.getInsideLeg(), 78.0);
            }

        }


    }

}