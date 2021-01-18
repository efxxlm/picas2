import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { ContractualControversyService } from 'src/app/core/_services/ContractualControversy/contractual-controversy.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-control-y-tabla-actuaciones-no-tai',
  templateUrl: './control-y-tabla-actuaciones-no-tai.component.html',
  styleUrls: ['./control-y-tabla-actuaciones-no-tai.component.scss']
})
export class ControlYTablaActuacionesNoTaiComponent implements OnInit {
  public controversiaID = parseInt(localStorage.getItem("controversiaID"));
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  displayedColumns: string[] = [
    'fechaActuacion',
    'actuacion',
    'numeroActuacion',
    'estadoRegistro',
    'estadoActuacion',
    'gestion',
  ];
  dataTable: any[] = [];  
  constructor(public dialog: MatDialog, private services: ContractualControversyService, private router: Router) {
  }

   ngOnInit(): void {
    this.services.GetListGrillaControversiaActuacion(this.controversiaID).subscribe(data=>{
      this.dataTable = data;
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    });
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  enviarComiteTecnicoTramAct(id){
    this.services.CambiarEstadoActuacionSeguimiento(id,'2').subscribe((data:any)=>{
      if(data.isSuccessful==true){
        this.ngOnInit();
      }
    });
  }
  verDetalleEditarActuacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleEditarActuacionNoTai',id]);
  }
  deleteActuacion(id) {
    this.openDialogSiNo("","¿Está seguro de eliminar este registro?",id);
  }
  eliminarActuacion(id){
    this.services.EliminarControversiaActuacion(id).subscribe((data:any)=>{
      this.ngOnInit();
    });
  }
  verDetalleActuacion(id){
    this.router.navigate(['/gestionarTramiteControversiasContractuales/verDetalleActuacionNoTai',id]);
  }
  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.eliminarActuacion(e);
      }
    });
  }
}
