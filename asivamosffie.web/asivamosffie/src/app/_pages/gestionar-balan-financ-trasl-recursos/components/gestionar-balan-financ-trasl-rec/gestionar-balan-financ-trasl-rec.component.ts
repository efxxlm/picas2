import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { FinancialBalanceService } from 'src/app/core/_services/financialBalance/financial-balance.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

export interface RegistrarInterface {
  fechaTerminacionProyecto: Date;
  llaveMen: string;
  tipoIntervencion: string;
  institucionEducativa: string;
  sedeEducativa: string;
  proyectoId: number;
  numeroTraslado: number;
  estadoBalance: string;
  registroCompleto: boolean;
  estadoBalanceCodigo: string;
}

@Component({
  selector: 'app-gestionar-balan-financ-trasl-rec',
  templateUrl: './gestionar-balan-financ-trasl-rec.component.html',
  styleUrls: ['./gestionar-balan-financ-trasl-rec.component.scss']
})
export class GestionarBalanFinancTraslRecComponent implements OnInit {
  verAyuda = false;

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  ELEMENT_DATA: RegistrarInterface[] = [];

  displayedColumns: string[] = [
    'fechaTerminacionProyecto',
    'llaveMEN',
    'tipoIntervencion',
    'institucionEducativa',
    'sede',    
    'numeroTraslados',
    'estadoBalance',
    'gestion'
  ];
  dataSource = new MatTableDataSource<RegistrarInterface>(this.ELEMENT_DATA);

  dataTable: any[] = [
    {
      fechaTerminacionProyecto: '09/08/2021',
      llaveMEN: 'LL457326',
      tipoIntervencion: 'Remodelación',
      institucionEducativa: 'I.E Nuestra Señora Del Carmen/Única sede',
      sede: 'Única sede',
      numeroTraslados: '---',
      estadoBalance: 'Sin balance validado',
      gestion: 1
    },
  ];
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
  constructor(
    private routes: Router,
    private financialBalanceService: FinancialBalanceService,
    public dialog: MatDialog,
  ) { }


  ngOnInit(): void {
    this.getAllReports();
    this.loadDataSource();
  }

  getAllReports() {
    this.financialBalanceService.gridBalance().subscribe(report => {
      this.dataSource.data = report as RegistrarInterface[];
    });
  }

  loadDataSource() {
    //this.dataSource = new MatTableDataSource(this.dataTable);
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
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
    };
  }
  validarBalance(id){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/validarBalance', id]);
  }
  verDetalleEditarBalance(id){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/verDetalleEditarBalance', id]);
  }
  verDetalleBalance(id){
    this.routes.navigate(['/gestionarBalanceFinancieroTrasladoRecursos/verDetalleBalance', id]);
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  aprobarBalance(pProyectoId: number) {
    this.financialBalanceService.approveBalance(pProyectoId).subscribe(respuesta => {
      this.openDialog('', '<b>La información ha sido guardada exitosamente.</b>');
      this.routes.navigateByUrl( '/', {skipLocationChange: true} )
      .then( () => this.routes.navigate( ['/gestionarBalanceFinancieroTrasladoRecursos'] ) );
    });
  }
}
