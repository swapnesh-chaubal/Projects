package com.example.swapneshchaubal.vieravoicecontrol;

import android.os.AsyncTask;

import java.io.BufferedOutputStream;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.HttpURLConnection;
import java.net.URL;
import java.nio.charset.Charset;
import java.util.Dictionary;
import java.util.Hashtable;

/**
 * Created by swapneshchaubal on 7/28/15.
 */
public class VoiceControl {
    Hashtable<String, String> mapping;

    public VoiceControl() {
        mapping = new Hashtable<String, String>();

        mapping.put("submenu", "NRC_SUBMENU-ONOFF"); // Option
        mapping.put("return", "NRC_RETURN-ONOFF"); // Return
        mapping.put("enter", "NRC_ENTER-ONOFF"); // Control Center click / enter
        mapping.put("ok", "NRC_ENTER-ONOFF"); // Control Center click / enter
        mapping.put("right", "NRC_RIGHT-ONOFF"); // Control RIGHT
        mapping.put("left", "NRC_LEFT-ONOFF"); // Control LEFT
        mapping.put("up", "NRC_UP-ONOFF"); // Control UP
        mapping.put("down", "NRC_DOWN-ONOFF"); // Control DOWN
        mapping.put("3d", "NRC_3D-ONOFF"); // 3D button
        mapping.put("menu", "NRC_MENU-ONOFF"); // Menu
        mapping.put("info", "NRC_INFO-ONOFF"); // info
        mapping.put("power", "NRC_POWER-ONOFF"); // Power off
        mapping.put("turn off", "NRC_POWER-ONOFF"); //power off
        mapping.put("off", "NRC_POWER-ONOFF"); //power off
        mapping.put("tv off", "NRC_POWER-ONOFF"); //power off
        mapping.put("hdmi", "NRC_CHG_INPUT-ONOFF");
        mapping.put("volume down", "NRC_VOLDOWN-ONOFF");
        mapping.put("volume down", "NRC_VOLUP-ONOFF");
    }

    public void ProcessSpeech(String speech) throws UnsupportedOperationException, IndexOutOfBoundsException{
        if(speech == null || speech.isEmpty())
            return;

        speech = speech.toLowerCase();
        String[] arrWords = speech.split(" ");

        if(arrWords.length == 0)
            return;
        speech = speech.trim();
        if(mapping.containsKey(speech)) {
            new sendMessageProxy().execute(mapping.get(speech));
        }
    }
}

// This is an async task to send data to a proxy server which redirects the request
// to the tv. There is some issue with the request being sent directly to the TV.
// Bad request is returned. The code for sending the request directly to the tv
// is below. When it is fixed, we won't need this class
class sendMessageProxy extends AsyncTask<String, Void, Void> {
    private final String ServerAddr = "http://192.168.1.134:9000";

    @Override
    protected Void doInBackground(String... msg) {
        try {
            URL server = new URL(ServerAddr);
            String actionString = String.format("{\"action\": \"%s\"}", msg[0]);
            SendCommand(server, actionString);
        }
        catch (Exception e){
            e.printStackTrace();
        }
        return null;
    }

    private void SendCommand(URL server, String action) {

        try {
            HttpURLConnection conn = (HttpURLConnection) server.openConnection();
            conn.setConnectTimeout(3000);
            conn.setReadTimeout(3000);
            conn.setDoOutput(true); //post data

            byte[] buff = action.getBytes(Charset.forName("UTF-8"));
            OutputStream out = new BufferedOutputStream(conn.getOutputStream());
            out.write(buff);
            out.flush();
            InputStream response = conn.getInputStream();

        }
        catch (Exception e)
        {
            String ex = e.getMessage();
        }
    }
}

// Class to send request directly to the TV. Not working yet.
class sendMessage extends AsyncTask<String, Void, Void> {

    @Override
    protected Void doInBackground(String... msg) {
        try {

            URL server = new URL(msg[0]);
            SendCommand(server,"");

        } catch (Exception e) {
            e.printStackTrace();
        }

        return null;
    }

    private String SendCommand(URL server, String action) {

        try {
            HttpURLConnection conn = (HttpURLConnection) server.openConnection();
            conn.setConnectTimeout(3000);
            conn.setReadTimeout(3000);
            conn.setDoOutput(true); //post data
            conn.setRequestMethod("POST");
            conn.setUseCaches(false);
            conn.setDoInput(true);
            //conn.setFixedLengthStreamingMode(324);
            conn.setRequestProperty("Accept", "text/xml");
            conn.setRequestProperty("Pragma", "no-cache");
            conn.setRequestProperty("SOAPACTION", "urn:panasonic-com:service:p00NetworkControl:1#X_SendKey");
            conn.setRequestProperty("User-Agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_10_3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/44.0.2403.107 Safari/537.36");
            conn.setRequestProperty("Cache-Control", "no-cache");
            conn.setRequestProperty("Origin", "chrome-extension://hgmloofddffdnphfgcellkdfbfbjeloo");
            conn.setRequestProperty("SOAPACTION", "urn:panasonic-com:service:p00NetworkControl:1#X_SendKey");
            conn.setRequestProperty("Content-Type", "text/xml;charset=\"UTF-8\"");
            conn.setRequestProperty("Accept-Encoding", "gzip, deflate");
            conn.setRequestProperty("Accept-Language", "en-US,en;q=0.8");


            StringBuilder body =  new StringBuilder();
            body.append("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            body.append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">");
            body.append("<s:Body>");
            body.append("<u:X_SendKey xmlns:u=\"urn:panasonic-com:service:p00NetworkControl:1\">");
            body.append("<X_KeyEvent>NRC_VOLDOWN-ONOFF</X_KeyEvent>");
            body.append("</u:X_SendKey>");
            body.append("</s:Body>");
            body.append("</s:Envelope>");

            conn.setRequestProperty("Content-Length", Integer.toString(body.toString().length()));
            byte[] buff = body.toString().getBytes(Charset.forName("UTF-8"));
            //OutputStreamWriter writer = new OutputStreamWriter(conn.getOutputStream());
            //writer.write(body.toString());
            OutputStream out = new BufferedOutputStream(conn.getOutputStream());
            out.write(buff);
            //out.write(new byte[0]);
            out.flush();

            int status = conn.getResponseCode();
            InputStream response;
            if(status >= HttpURLConnection.HTTP_BAD_REQUEST)
                response = conn.getErrorStream();
            else
                response = conn.getInputStream();

            return "";
        }
        catch (Exception e)
        {
            return e.getMessage();
        }
    }
}