import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-tabla-otros-descuentos-og-gbftrec',
  templateUrl: './tabla-otros-descuentos-og-gbftrec.component.html',
  styleUrls: ['./tabla-otros-descuentos-og-gbftrec.component.scss']
})
export class TablaOtrosDescuentosOgGbftrecComponent implements OnInit {
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  @Input() tablaOtroDescuento: any;
  displayedColumns: string[] = ['aportante', 'conceptoPago', 'descuento', 'valorDescuento', 'valorTotal'];
  dataTable: any[];
  valorTotal: any[] = [];
  constructor() {}

  ngOnInit(): void {
    this.calcValorTotal();
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataTable = this.tablaOtroDescuento;
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }

  calcValorTotal() {
    for (let variable of this.tablaOtroDescuento) {
      var aportante = [];
      for (let variable2 of variable.listDyAportante) {
        var contadorValorTotal = 0;
        for (let variable3 of variable2.listDyDescuento) {
          contadorValorTotal += variable3.valorDescuento;
        }
        aportante.push(contadorValorTotal);
      }
      this.valorTotal.push({aportante: aportante})
    }
  }
}
