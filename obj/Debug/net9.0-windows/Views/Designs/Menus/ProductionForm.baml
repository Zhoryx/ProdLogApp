   M S B A M L   `   `   `  ����  
ProdLogApp%    ProdLogApp.Views.ProductionForm   5   	   W SPresentationCore, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=9.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=9.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Agregar Producción��5   	   $	��400��5   	   $	��600��5   	    � WindowStartupLocation$ CenterScreen=�5   	    � 
ResizeMode$ NoResize=�5   	   ���#0864ac � 	Resources 5       �� MergedDictionaries �� Source�� 5   
    5      �� 5      $ /Views/Designs/Style.xaml?�6%   %    5      .��5      � $��	10,0,0,-2q�6    � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6    � ColumnDefinitions 5"   
   �� 5#      $
��Auto �6   �� 5$      $	��53* �6   �� 5%      $	��30* �6   .+�5)   
   �� -   5         ��Name$ ProductoLabel��5)      $��	Producto:��6'   $��16(�5*      ���#FFFFFF$	��Top=�5+      $��0��6(   $��0��65   $��	0,46,10,0q�5,      $��31��6#   �� 5-   
   -   5           ModernButtonStyle#��[  5-      $��Seleccionar��6<   $��0��5.      $��1��6   $	��NaN��6.   $��25��6:   $��167,23,7,50q�6F   5/      �� 51   
   -   5        	 ��Name$	 ProductoTextBox��51        ModernTextBoxStyle#��[ 6+   $��1��6W   $��56,42,58,17q�52   
   $��2��6   �� 55   
   $��Hora Inicio:��6   $��14(�6'   ���#FFFFFF$��Center=�56      $��1��6+   $��0��68   $��0,0,10,0q�57      $��28��6"   �� 58   
   -   5        $	 HoraInicioTextBox��58      #��[ 6-   $��1��6Y   $��57,92,58,53q�59      $��2��6'   $��3��6;   �� 5<   
   $��	Hora Fin:��6   $��14(�6$   ���#FFFFFF$��Center=�5=      $��2��6+   $��0��68   $��0,0,10,0q�5>      $��28��6"   �� 5?   
   -   5        $	 HoraFinTextBox��5?      #��[ 6*   $��2��6V   $��1��6c   $��57,10,58,10q�5@      $��2��6'   �� 5C   
   $��Cantidad Producida:��6   $��14(�6.   ���#FFFFFF$��Center=�5D      $��3��6+   $��0��68   $��0,0,10,0q�5E      $��28��6"   �� 5F   
   -   5        $	 CantidadTextBox��5F      #��[ 6+   $��3��6W   $��1��6d   $��57,10,58,10q�5G      $��2��6'   �� 5J   
   
 �� Orientation$
 
Horizontal=�6   $
��Left=�6.   $��4��5K      $��3��6"   $��120,10,0,10q�66   $	��439��6K   .+�5L      �� -   5          ConfirmButtonStyle#��[ 5L      $��35��6A   $��	Confirmar��6M   $��10q�5M      6!   �� 5N      -   5          CancelButtonStyle#��[ 5N      $��35��6@   $��Cancelar��6L   $��10q�5O      6!   5P      5Q      5R      