import sys
import pandas as pd
import numpy as np
import matplotlib.pyplot as plt
from scipy.ndimage import gaussian_filter1d

def plot_response_times(csv_filename, sigma=2):
    # Load data from CSV file
    data = pd.read_csv(csv_filename)

    # Extract modules (columns)
    modules = data.columns[1:]  # Skip the first column which is 'Experiment Index'
    num_samples = len(data)

    # Initialize the plot
    plt.figure(figsize=(12, 7))

    # Plot each module with different lines
    for module in modules:
        response_times = data[module].values
        # Apply Gaussian smoothing
        smoothed_response_times = gaussian_filter1d(response_times, sigma=sigma)
        plt.plot(data['Experiment Index'], smoothed_response_times, linewidth=1.5, label=module)

    # Add labels and titles
    plt.title('User Interaction Responsiveness by Module', fontsize=16)
    # plt.xlabel('Experiment Index (times)', fontsize=14)
    plt.xlabel('Experiment Index (times)', fontsize=19) # alex revised
    # plt.ylabel('Response Time (ms)', fontsize=14, labelpad=0)  # Set labelpad to 0
    plt.ylabel('Response Time (ms)', fontsize=19, labelpad=0)  # alex revised
    plt.gca().xaxis.set_label_coords(0.5, -0.035)  # Adjust y-axis label position
    plt.gca().yaxis.set_label_coords(-0.035, 0.5)  # Adjust y-axis label position

    # Set x-axis limits to remove margins
    plt.xlim(data['Experiment Index'].min(), data['Experiment Index'].max())

    # Adding grid and legend
    plt.grid(True, which='both', linestyle='--', linewidth=0.5)
    # plt.legend(loc='upper left', fontsize=10, ncol=2)
    plt.legend(loc='upper left', fontsize=17, ncol=2) # alex revised

    # Highlight the average response time across all modules
    mean_response_time = data[modules].apply(lambda x: gaussian_filter1d(x, sigma=sigma)).mean(axis=1)
    plt.plot(data['Experiment Index'], mean_response_time, color='k', linewidth=2.5, label='Mean Response Time', linestyle='--')

    # Show plot
    plt.tight_layout()
    plt.show()

if __name__ == "__main__":
    if len(sys.argv) != 2 and len(sys.argv) != 3:
        print("Usage: python main.py <path_of_the_csv_file> [sigma]")
        sys.exit(1)

    csv_filename = sys.argv[1]
    sigma = float(sys.argv[2]) if len(sys.argv) == 3 else 0.1
    plot_response_times(csv_filename, sigma)
