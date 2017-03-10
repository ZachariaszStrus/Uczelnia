/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package proj4;

import java.io.IOException;
import javafx.application.Application;
import javafx.fxml.FXMLLoader;
import javafx.scene.Scene;
import javafx.scene.layout.Pane;
import javafx.stage.Stage;

/**
 *
 * @author Zaki
 */
public class Proj4 extends Application {
    
    @Override
    public void start(Stage primaryStage) throws IOException {
        Pane mainPane = FXMLLoader.load(getClass().getResource("MainWindow.fxml"));
        Scene scene = new Scene(mainPane);
        
        primaryStage.setTitle("Application 4");
        primaryStage.setScene(scene);
        primaryStage.show();
    }

    /**
     * @param args the command line arguments
     */
    public static void main(String[] args) {
        launch(args);
    }
    
}
