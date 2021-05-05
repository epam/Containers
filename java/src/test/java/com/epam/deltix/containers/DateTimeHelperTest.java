package com.epam.deltix.containers;

import org.junit.Test;

import java.util.Date;

/**
 * Created by DriapkoA on 03/01/2018.
 */
public class DateTimeHelperTest {

    @Test
    public void nowTimeTest() {

        long now = System.currentTimeMillis();
        MutableString str1 = new MutableString();
        MutableString str2 = new MutableString();


        str1.assign(new Date(now).toString());
        DateTimeHelper.assign(str2, now);

        //     Assert.assertEquals(true, str1.equals(str2));


    }

}
