# snake
貪食蛇遊戲與手機操控介面如下      
<img src="https://github.com/wahaha829/snake/blob/master/%E8%B2%AA%E9%A3%9F%E8%9B%87.png" width="50%" height="50%">
<img src="https://github.com/wahaha829/snake/blob/master/Screenshot_20201025-205115.jpg" width="10%" height="10%">
<img src="https://github.com/wahaha829/snake/blob/master/Screenshot_20201025-205027.jpg" width="10%" height="10%">     
預設使用鍵盤WASD控制蛇移動方向，    
或使用手機連接Arduino上的HC-05藍牙模組後，由Serial Port傳入字元來控制。    

用二維陣列定義蛇與食物的所在座標，     
List定義蛇的身體與食物，方便動態add、remove，    
使用計時器與新增頭部、刪除尾巴製造出蛇移動的效果，  
並使用Windows提供的GDI+繪圖功能畫出蛇及食物。   

玩家可使用trackbar改變計時器interval達到調整移動速度、      
或使用combobox設定食物數量。   
