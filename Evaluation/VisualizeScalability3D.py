import pandas as pd
import matplotlib.pyplot as plt
from mpl_toolkits.mplot3d import Axes3D
import sys

def identify_files(file1, file2):
    # Load the first file to check its columns
    df1 = pd.read_csv(file1)
    # Load the second file to check its columns
    df2 = pd.read_csv(file2)
    
    # Identify file types based on columns
    if 'Latency (ms)' in df1.columns:
        latency_df = df1
        utilization_df = df2
    else:
        latency_df = df2
        utilization_df = df1
    
    return latency_df, utilization_df

def main(file1, file2):
    # Identify which file contains latency data and which contains utilization data
    latency_df, utilization_df = identify_files(file1, file2)

    # Extract data
    num_inputs = latency_df['Number of Video Inputs']
    latency = latency_df['Latency (ms)']
    cpu_utilization = utilization_df['CPU Utilization (%)']
    gpu_utilization = utilization_df['GPU Utilization (%)']

    # Create a 3D plot
    fig = plt.figure(figsize=(14, 10))
    ax = fig.add_subplot(111, projection='3d')

    # Plotting Latency, CPU, and GPU Utilization
    sc = ax.scatter(num_inputs, latency, cpu_utilization, c=gpu_utilization, cmap='viridis', s=50)

    # Adding labels
    ax.set_xlabel('Number of Video Inputs (n)', fontsize=16)
    ax.set_ylabel('Latency (ms)', fontsize=16)
    ax.set_zlabel('CPU Utilization (%)', fontsize=16)
    ax.set_title('3D Visualization of Latency and Utilization vs. Number of Video Inputs', fontsize=18)

    # Adding color bar which maps values to colors
    cbar = plt.colorbar(sc)
    cbar.set_label('GPU Utilization (%)', fontsize=16)
    
    # Display the plot
    plt.show()

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python main.py <file1.csv> <file2.csv>")
        sys.exit(1)

    file1 = sys.argv[1]
    file2 = sys.argv[2]
    main(file1, file2)
