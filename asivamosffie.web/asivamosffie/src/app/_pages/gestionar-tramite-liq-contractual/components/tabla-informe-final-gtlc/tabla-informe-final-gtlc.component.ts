import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';

@Component({
  selector: 'app-tabla-informe-final-gtlc',
  templateUrl: './tabla-informe-final-gtlc.component.html',
  styleUrls: ['./tabla-informe-final-gtlc.component.scss']
})
export class TablaInformeFinalGtlcComponent implements OnInit {
  @Input() verDetalleBtn;
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaEnvioInformeFinal',
    'fechaAprobacionInformeFinal',
    'llaveMen',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',
    'estadoVerificacion',
    'gestion'
  ];
  dataTable: any[] = [
    {
      fechaEnvioInformeFinal: '29/11/2020',
      fechaAprobacionInformeFinal: '29/11/2020',
      llaveMen: 'LL457326',
      tipoIntervencion: 'Remodelación',
      institucionEducativa: 'I.E. Nuestra Señora del Carmen',
      sede: 'Única sede',
      estadoVerificacion: 'Sin verificacion',
      id: 1
    },
  ];
  constructor(private router: Router) { }

  ngOnInit(): void {
    this.loadDataSource();
  }
  loadDataSource() {
    this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
  }
  verDetalleInformeFinal(id) {
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/detalleInformeFinal', id]);
  }
  verificarInformeFinal(id) {
    this.router.navigate(['/gestionarTramiteLiquidacionContractual/verificarInformeFinal', id]);
  }

}
