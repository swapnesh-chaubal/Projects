package com.example.swapneshchaubal.vieravoicecontrol;

import android.content.ActivityNotFoundException;
import android.content.Intent;
import android.speech.RecognizerIntent;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Toast;

import java.util.ArrayList;

// TODO: Handle rotation of mobile. This still works since we don't need to save any state
public class MainActivity extends ActionBarActivity {
    protected static final int RESULT_SPEECH = 1;
    VoiceControl voiceControl;
    boolean startListening;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        voiceControl = new VoiceControl();
        startListening = false; // flag to call the listener again. Turned off when the user says bye
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }

    // Event handler for the speak button click. Creates a new intent to recognize speech
    // by putting a few details about the intent.
    public void buttonSpeakClick(View button){

        Intent intent = new Intent(
                RecognizerIntent.ACTION_RECOGNIZE_SPEECH);

        intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, "en-US");

        if(!startListening)
            startListening = true;
        else {
            startListening = false;
            return;
        }

        try{
            startActivityForResult(intent, RESULT_SPEECH);
        }
        catch (ActivityNotFoundException e){
            Toast toast = Toast.makeText(getApplicationContext(), "Sorry, " +
                            "Aplication doesnt support Speech To Text  ",
                    Toast.LENGTH_SHORT);
            toast.show();
        }
    }

    // Gets the user's speech and passes it to
    // the VoiceControl object for processing
    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        super.onActivityResult(requestCode, resultCode, data);

        try{

            switch (requestCode) {
                case RESULT_SPEECH: {
                    if (resultCode == RESULT_OK && null != data) {

                        ArrayList<String> text = data
                                .getStringArrayListExtra(RecognizerIntent.EXTRA_RESULTS);

                        String voiceCmd = text.get(0);
                        if(voiceCmd.startsWith("by")){
                            startListening = false;
                            return;
                        }
                        else {
                            voiceControl.ProcessSpeech(text.get(0));
                        }

                        if(startListening) {
                            Intent intent = new Intent(
                                    RecognizerIntent.ACTION_RECOGNIZE_SPEECH);

                            intent.putExtra(RecognizerIntent.EXTRA_LANGUAGE_MODEL, "en-US");
                            startActivityForResult(intent, RESULT_SPEECH);
                        }
                    }
                    break;
                }

            }
        }
        catch (Exception e){
            e.printStackTrace();
        }
    }
}



