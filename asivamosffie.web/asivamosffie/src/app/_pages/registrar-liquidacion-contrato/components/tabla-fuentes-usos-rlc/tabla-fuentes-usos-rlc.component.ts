import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-tabla-fuentes-usos-rlc',
  templateUrl: './tabla-fuentes-usos-rlc.component.html',
  styleUrls: ['./tabla-fuentes-usos-rlc.component.scss']
})
export class TablaFuentesUsosRlcComponent implements OnInit {

  @Input() contratoId: any[] = [];
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['uso', 'fuente', 'aportante', 'valorUso', 'saldoActualUso'];
  dataTable: any[] = [];
  data = [];

  constructor(
    private financialBalanceService: FinancialBalanceService
  ) { }

  ngOnInit(): void {
    this.getTablaUsoFuenteAportanteXContratoId();
  }

  getTablaUsoFuenteAportanteXContratoId() {
    this.financialBalanceService.getTablaUsoFuenteAportanteXContratoId(this.contratoId).subscribe(data => {
      this.data = data.usos;
      if (this.data) {
        this.data.forEach( registro => {
          const aportantes = []
          const valorUso = [];
          const saldoActualUso = [];

          registro.fuentes.forEach( fuente => {
              aportantes.push( fuente.aportante[ 0 ] )
              valorUso.push( fuente.aportante[ 0 ].valorUso[ 0 ].valor )
              saldoActualUso.push( fuente.aportante[ 0 ].valorUso[ 0 ].valorActual );
          } )

          const registroObj = {
              nombreUso: registro.nombreUso,
              fuentes: registro.fuentes,
              aportante: aportantes,
              valorUso,
              saldoActualUso
          }

          this.dataTable.push( registroObj );
        })
      }

      this.loadDataSource();

    })
  }

  loadDataSource() {
    this.dataSource = new MatTableDataSource( this.dataTable );
    this.dataSource.sort = this.sort;
  }

}
