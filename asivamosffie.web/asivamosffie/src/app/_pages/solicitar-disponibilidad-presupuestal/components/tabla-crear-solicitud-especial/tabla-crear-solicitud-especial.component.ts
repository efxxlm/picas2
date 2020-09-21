import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-tabla-crear-solicitud-especial',
  templateUrl: './tabla-crear-solicitud-especial.component.html',
  styleUrls: ['./tabla-crear-solicitud-especial.component.scss']
})
export class TablaCrearSolicitudEspecialComponent implements OnInit {

  displayedColumns: string[] = [
    'fecha',
    'numero',
    'valorSolicitado',
    'estadoSolicitud',
    'estadoRegistro',
    'id'
  ];
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    private budgetAvailabilityService: BudgetAvailabilityService,
    public dialog: MatDialog,
    private router: Router,


  ) { }

  ngOnInit(): void {

    this.budgetAvailabilityService.getDDPEspecial()
      .subscribe( listaDDP => {
        this.dataSource = new MatTableDataSource(listaDDP);
      })

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
    this.paginator._intl.getRangeLabel = (page, pageSize, length) => {
      if (length === 0 || pageSize === 0) { return '0 de ' + length; }
      length = Math.max(length, 0);
      const startIndex = page * pageSize;
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
      return startIndex + 1 + ' - ' + endIndex + ' de ' + length;
    };
  }

  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  enviarSolicitud(e: number) {
    
    this.budgetAvailabilityService.sendRequest( e )
      .subscribe( respuesta => {
        this.openDialog( '', respuesta.message );
        if (respuesta.code == "200")
          this.ngOnInit();
      })
  }

  editar(e: number) {
    console.log(e);
    this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudEspecial/nueva',e]);
  }

  eliminar(e: number) {
    this.budgetAvailabilityService.eliminarDisponibilidad( e )
      .subscribe( respuesta => {
        this.openDialog( '', respuesta.message );
        if (respuesta.code == "200")
          this.ngOnInit();
      })
  }

}
