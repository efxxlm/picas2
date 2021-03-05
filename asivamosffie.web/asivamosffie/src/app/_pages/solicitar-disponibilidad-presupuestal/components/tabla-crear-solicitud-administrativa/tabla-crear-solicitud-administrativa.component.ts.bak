import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { BudgetAvailabilityService } from 'src/app/core/_services/budgetAvailability/budget-availability.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';


@Component({
  selector: 'app-tabla-crear-solicitud-administrativa',
  templateUrl: './tabla-crear-solicitud-administrativa.component.html',
  styleUrls: ['./tabla-crear-solicitud-administrativa.component.scss']
})
export class TablaCrearSolicitudadministrativaComponent implements OnInit {

  displayedColumns: string[] = [
    'fecha',
    'numero',
    'valorSolicitado',
    'estadoSolicitud',
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

    this.budgetAvailabilityService.getDDPAdministrativa()
      .subscribe( listaDDP => {
        //console.log( listaDDP )
        this.dataSource = new MatTableDataSource(listaDDP);
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
      })

    
  }

  openDialog(modalTitle: string, modalText: string,reload:boolean=false) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
    dialogRef.afterClosed().subscribe(result => {
      if(reload)
        location.reload();
    })
  }

  enviarSolicitud(e: number) {
    
    this.budgetAvailabilityService.sendRequest( e )
      .subscribe( respuesta => {
        this.openDialog( '', `<b>${respuesta.message}</b>` );
        if (respuesta.code == "200")
          this.ngOnInit();
      })
  }

  editar(e: number) {
    console.log(e);
    this.router.navigate(['/solicitarDisponibilidadPresupuestal/crearSolicitudAdministrativa/nueva', e]);
  }

  openDialogSiNo(modalTitle: string, modalText: string, e:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText,siNoBoton:true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if(result === true)
      {
        this.budgetAvailabilityService.eliminarDisponibilidad( e )
        .subscribe( respuesta => {
          console.log(respuesta);
        this.openDialog( '', `<b>${respuesta.message}</b>` ,true);
        if (respuesta.code == "200")
          this.ngOnInit();
      })
      }
    });
  }

  eliminar(e: number) {
    this.openDialogSiNo('', '<b>¿Está seguro de eliminar este registro?</b>', e)
  }
  verDetalle(e: number){
    this.router.navigate(['/solicitarDisponibilidadPresupuestal/verDetalleDDPAdministrativo', e]);  
  }
}
