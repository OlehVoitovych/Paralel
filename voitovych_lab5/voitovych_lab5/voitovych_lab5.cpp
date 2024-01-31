#include <iostream>
#include <cmath>
#include <omp.h>
#include <thread>
#include <vector>

using namespace std;

double calculatePiSequential(int n) {
    double pi = 0.0;
    for (int i = 0; i < n; i++) {
        double term = 1.0 / (2 * i + 1);
        if (i % 2 == 0) {
            pi += term;
        }
        else {
            pi -= term;
        }
    }
    return 4.0 * pi;
}

void calculatePiParallel(int chunk_size, double& pi, int thrIndex) {
    int start = thrIndex * chunk_size;
    int end = start + chunk_size;
    for (int i = start; i < end; i++) {
        double term = 1.0 / (2 * i + 1);
        if (i % 2 == 0) {
            pi += term;
        }
        else {
            pi -= term;
        }
    }
}


int main() {
    /*int n;
    int num_threads;
    cin >> n;
    cin >> num_threads;*/
    int n = 10000000;
    int const num_threads = 50;
    int chunk_size = n / num_threads;
    double pi = 0.0;

    double startTimes = omp_get_wtime();
    double pi_seq = calculatePiSequential(n);
    double endTimes = omp_get_wtime();


    vector<thread> ths;
    for (int i = 0; i < num_threads; i++)
    {
        thread t(calculatePiParallel, chunk_size, ref(pi), i);
        ths.push_back(move(t));
    }

    double startTimep = omp_get_wtime();
    for (thread& th : ths)
    {
        th.join();
    }
    double endTimep = omp_get_wtime();
    cout << "calculate Pi Sequential: " << pi_seq << " time: " << endTimes - startTimes << "\ncalculate Pi Parallel: " << pi * 4.0 << " time: " << endTimep - startTimep;

}


