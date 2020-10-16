import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { CofinanciacionService, Cofinanciacion } from 'src/app/core/_services/Cofinanciacion/cofinanciacion.service';
import { Router } from '@angular/router';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { MatDialog } from '@angular/material/dialog';


@Component({
  selector: 'app-tabla-acuerdos',
  templateUrl: './tabla-acuerdos.component.html',
  styleUrls: ['./tabla-acuerdos.component.scss']
})
export class TablaAcuerdosComponent implements OnInit {

  displayedColumns: string[] = ['fechaCreacion', 'numeroAcuerdo', 'vigenciaAcuerdo', 'valorTotal', 'estadoRegistro', 'id'];
  dataSource = new MatTableDataSource();
  listaCofinanciacion: Cofinanciacion[] = [];

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, {static: true}) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor( private cofinanciacionService: CofinanciacionService,
               private router: Router,public dialog: MatDialog, ) { }

  ngOnInit(): void {

    this.cofinanciacionService.listaAcuerdosCofinanciacion().subscribe( cof => 
      {
         this.listaCofinanciacion = cof; 
         this.listaCofinanciacion.forEach(element => {
          let fechaSesion = new Date(element.fechaCreacion);
            element.fechaCreacion = `${fechaSesion.getDate()}/${fechaSesion.getMonth() + 1}/${fechaSesion.getFullYear()}`
         });
         this.dataSource.data = this.listaCofinanciacion;
      } );

    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;
    this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    this.paginator._intl.nextPageLabel = 'Siguiente';
    this.paginator._intl.previousPageLabel = 'Anterior';
  }

  editarAcuerdo(e: number) {
    this.router.navigate(['/registrarAcuerdos', e ]);
  }
  eliminarAcuerdo(e: number) {
    this.openDialogSiNo("","¿Está seguro de eliminar este registro?",e);
  }
  eliminarAcuerdoConfirmado(e: number) {
    this.cofinanciacionService.EliminarCofinanciacionByCofinanciacionId(e).subscribe(result=>{
      if(result.code==="200")
      {
        this.openDialog("",result.message);
        this.dataSource.data=[];
        this.listaCofinanciacion=[];
        this.cofinanciacionService.listaAcuerdosCofinanciacion().subscribe( cof => 
          {
             this.listaCofinanciacion = cof; 
             this.dataSource.data = this.listaCofinanciacion;
          } );
      }
      else{
        this.openDialog("",result.message);      
      }
    });
  }
  openDialog(modalTitle: string, modalText: string) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }
  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result) {
        this.eliminarAcuerdoConfirmado(e);
      }
    });
  }

}
