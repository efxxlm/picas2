import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-balance-financiero-gtlc',
  templateUrl: './tabla-balance-financiero-gtlc.component.html',
  styleUrls: ['./tabla-balance-financiero-gtlc.component.scss']
})
export class TablaBalanceFinancieroGtlcComponent implements OnInit {
  @Input() verDetalleBtn;
  displayedColumns: string[] = ['fechaTerminacionProyecto', 'llaveMEN', 'tipoIntervencion', 'institucionEducativa', 'sede', 'numeroTraslados', 'gestion'];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  dataTable: any[] = [
    {
      fechaTerminacionProyecto: '09/08/2021',
      llaveMEN: 'LL457326',
      tipoIntervencion: 'Remodelación',
      institucionEducativa: 'I.E Nuestra Señora Del Carmen/Única sede',
      sede: 'Única sede',
      numeroTraslados: '1',
      estadoBalance: 'Sin balance validado',
      id: 1
    },
  ];
  constructor(private router: Router) { }

  ngOnInit(): void {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  verDetalle(id){
    //this.router.navigate(['/registrarLiquidacionContrato/detalleBalanceFinanciero', id]);
  }
  verificar(id){
    
  }
}
