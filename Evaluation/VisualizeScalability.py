import pandas as pd
import matplotlib.pyplot as plt
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

    # Calculate mean latency
    mean_latency = latency.mean()

    # Plotting Latency
    plt.figure(figsize=(12, 7))

    plt.subplot(2, 1, 1)  # Create a subplot for Latency
    plt.plot(num_inputs, latency, label='Latency (ms)', color='r', linewidth=2.0)
    plt.axhline(y=mean_latency, color='k', linestyle='--', linewidth=2.0, label=f'Mean Latency: {mean_latency:.2f} ms')
    plt.title('System Scalability: Latency and Utilization vs. Number of Video Inputs', fontsize=16)
    # plt.xlabel('Number of Video Inputs (n)', fontsize=12)
    # plt.ylabel('Latency (ms)', fontsize=12)
    plt.xlabel('Number of Video Inputs (n)', fontsize=19) # alex revised
    plt.ylabel('Latency (ms)', fontsize=19) # alex revised
    plt.gca().xaxis.set_label_coords(0.5, -0.08)  # Adjust y-axis label position
    plt.gca().yaxis.set_label_coords(-0.025, 0.5)  # Adjust y-axis label position
    plt.grid(True, linestyle='--', linewidth=0.5)
    plt.legend(loc='upper left')
    plt.ylim(bottom=0)  # Start y-axis from 0
    plt.xlim(left=0, right=num_inputs.max())  # Remove margin on the right side

    # Plotting CPU and GPU Utilization
    plt.subplot(2, 1, 2)  # Create a subplot for CPU/GPU utilization
    plt.plot(num_inputs, cpu_utilization, label='CPU Utilization (%)', color='b', linewidth=2.0)
    plt.plot(num_inputs, gpu_utilization, label='GPU Utilization (%)', color='g', linewidth=2.0)
    # plt.xlabel('Number of Video Inputs (n)', fontsize=12)
    # plt.ylabel('Utilization (%)', fontsize=12)
    plt.xlabel('Number of Video Inputs (n)', fontsize=19) # alex revised
    plt.ylabel('Utilization (%)', fontsize=19) # alex revised
    plt.gca().xaxis.set_label_coords(0.5, -0.08)  # Adjust y-axis label position
    plt.gca().yaxis.set_label_coords(-0.02, 0.5)  # Adjust y-axis label position
    plt.grid(True, linestyle='--', linewidth=0.5)
    plt.legend(loc='upper left')
    plt.ylim(bottom=0)  # Start y-axis from 0
    plt.xlim(left=0, right=num_inputs.max())  # Remove margin on the right side

    # Display the plots with no margins
    plt.tight_layout(pad=0.5, rect=[0, 0, 1, 1])  # Slimmer margins
    plt.show()

if __name__ == "__main__":
    if len(sys.argv) != 3:
        print("Usage: python main.py <file1.csv> <file2.csv>")
        sys.exit(1)

    file1 = sys.argv[1]
    file2 = sys.argv[2]
    main(file1, file2)
