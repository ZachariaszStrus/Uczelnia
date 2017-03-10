package proj4;

import java.io.File;
import java.util.logging.Level;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.stage.FileChooser;
import javax.xml.bind.JAXBContext;
import javax.xml.bind.JAXBException;
import javax.xml.bind.Unmarshaller;
import com.w3schools.Price;
import com.w3schools.Product;
import com.w3schools.ProductCatalog;
import java.net.URL;
import java.util.ResourceBundle;
import javafx.collections.FXCollections;
import javafx.collections.ObservableList;
import javafx.fxml.Initializable;
import javafx.scene.control.SelectionMode;
import javafx.scene.control.TableView;
import javafx.scene.control.TextField;
import java.util.ArrayList;
import java.util.List;
 
import javax.xml.bind.annotation.XmlElement;
import javax.xml.bind.annotation.XmlRootElement;
/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */

/**
 *
 * @author Zaki
 */
public class Controller implements Initializable {
    
     @FXML
    private TableView<Product> productTable;
    @FXML
    private TextField productName;
    @FXML
    private TextField productPrice;
    @FXML
    private TextField productCurrency;
    private ObservableList<Product> products = FXCollections.observableArrayList();
    
    @Override
    public void initialize(URL url, ResourceBundle rb) {
        productTable.setItems(products);
        productTable.getSelectionModel().setSelectionMode(SelectionMode.MULTIPLE);
        productTable.setEditable(true);
        loadDafault();
        
        setTableEditHandlers();
    }    
        @FXML
    private void loadProductsAction(ActionEvent event) throws JAXBException {
        FileChooser chooser = new FileChooser();
        File file = chooser.showOpenDialog(null);
        if (file != null) {

                JAXBContext context = JAXBContext.newInstance("catalogSchema");
                Unmarshaller unmarshaller = context.createUnmarshaller();
                ProductCatalog productCatalog = (ProductCatalog) unmarshaller.unmarshal(file);
                products.clear();
                products.addAll(productCatalog.getProduct());

        }
    }

    private void loadDafault() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }

    private void setTableEditHandlers() {
        throw new UnsupportedOperationException("Not supported yet."); //To change body of generated methods, choose Tools | Templates.
    }
    
}
