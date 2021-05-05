package com.epam.deltix.containers.constructor;

import org.openjdk.jmh.annotations.*;

import java.lang.reflect.Constructor;
import java.lang.reflect.InvocationTargetException;
import java.util.concurrent.TimeUnit;

@BenchmarkMode(Mode.AverageTime)
@OutputTimeUnit(TimeUnit.NANOSECONDS)
@Warmup(time = 5, timeUnit = TimeUnit.SECONDS, iterations = 1)
@Measurement(time = 5, timeUnit = TimeUnit.SECONDS, iterations = 1)
@State(Scope.Thread)
public class ConstructorBenchmark {
    private Constructor<PrivateReflection> privateCounstructor;

    @Setup(Level.Trial)
    public void setUp() throws NoSuchMethodException {
        privateCounstructor = PrivateReflection.class.getDeclaredConstructor();
        privateCounstructor.setAccessible(true);
    }

    @Benchmark
    public ExplicitCall testExplicitCall() {
        return new ExplicitCall();
    }

    @Benchmark
    public PrivateReflection testPrivateReflection() throws IllegalAccessException, InvocationTargetException, InstantiationException {
        return privateCounstructor.newInstance();
    }

    @Benchmark
    public PublicReflection testPublicReflection() throws IllegalAccessException, InstantiationException {
        return PublicReflection.class.newInstance();
    }
}
