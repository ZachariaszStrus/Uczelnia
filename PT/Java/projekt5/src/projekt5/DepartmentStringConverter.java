/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
package projekt5;

import javafx.util.StringConverter;
import DatabaseClasses.Department;
/**
 *
 * @author Zaki
 */
public class DepartmentStringConverter extends StringConverter<Department> {

    @Override
    public String toString(Department object) {
        if(object != null){
            return object.getShortcut();
        }
        return null;
    }

    @Override
    public Department fromString(String string) {
        return new Department();
    }
    
}
