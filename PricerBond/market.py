
import json
from math import sqrt
from random import uniform, gauss

# =============================================================================
# import matplotlib.pyplot as plt
# =============================================================================

DT = 1 / 365
SIMUL_NBR = 100
STOCK_NAME = "ACCOR"
STOCK = {
        "name": STOCK_NAME,
        "vol": uniform(0, 0.1),
        "drift": uniform(-0.1, 0.1),
        "spots": [100],
        }

def generateMarketData(nbr_simulation: int, stock):
    for _ in range(nbr_simulation):
        old_price = stock["spots"][-1]
        drift_amount = stock["drift"] * old_price * DT
        vol_amount = stock["vol"] * old_price * gauss(mu=0, sigma=sqrt(DT))
        change_amount = drift_amount + vol_amount
        new_price = old_price + change_amount
        stock["spots"].append(new_price)

def exportStock(stock):
# =============================================================================
#     with open(file_name, "w") as f:
#         f.write(f"{json.dumps(stock)}\n")
# =============================================================================
    return (json.dumps(stock))

# =============================================================================
# def showMarketData(stock):
#     spots = stock["spots"]
#     plt.plot(list(range(len(spots))), spots)
#     plt.show()
#     print(json.dumps(stock))
# =============================================================================

generateMarketData(SIMUL_NBR, STOCK)
exportStock(STOCK)
# =============================================================================
# showMarketData(STOCK)
# =============================================================================

