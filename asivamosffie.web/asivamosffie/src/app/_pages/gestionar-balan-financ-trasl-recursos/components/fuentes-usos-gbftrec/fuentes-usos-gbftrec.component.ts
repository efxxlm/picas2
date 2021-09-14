import { Component, OnInit, ViewChild, Input } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';

@Component({
  selector: 'app-fuentes-usos-gbftrec',
  templateUrl: './fuentes-usos-gbftrec.component.html',
  styleUrls: ['./fuentes-usos-gbftrec.component.scss']
})
export class FuentesUsosGbftrecComponent implements OnInit {

  @Input() contratoId: any[] = [];
  @Input() pProyectoId: number;
  dataSource = new MatTableDataSource();
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = ['uso', 'fuente', 'aportante', 'valorUso', 'saldoActualUso'];
  dataTable: any[] = [];
  data = [];
  
  constructor(
    private financialBalanceService: FinancialBalanceService
  ) { }

  ngOnInit(): void {
    this.getTablaUsoFuenteAportanteXContratoIdXProyectoId();
  }
  
  getTablaUsoFuenteAportanteXContratoIdXProyectoId() {
    this.financialBalanceService.getTablaUsoFuenteAportanteXContratoIdXProyectoId(this.contratoId, this.pProyectoId).subscribe(data => {
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
    //this.dataSource.paginator = this.paginator;
    //this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    /*
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) {
        return '0 de ' + length;
      }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      // If the start index exceeds the list length, do not try and fix the end index to the end.
      const endIndex = startIndex < length ?
        Math.min(startIndex + pageSize, length) :
        startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };*/
  }

}
