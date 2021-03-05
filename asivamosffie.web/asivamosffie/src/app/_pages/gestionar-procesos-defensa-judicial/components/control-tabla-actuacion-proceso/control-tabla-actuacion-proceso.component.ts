import { Component, Input, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { DefensaJudicialService } from 'src/app/core/_services/defensaJudicial/defensa-judicial.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-control-tabla-actuacion-proceso',
  templateUrl: './control-tabla-actuacion-proceso.component.html',
  styleUrls: ['./control-tabla-actuacion-proceso.component.scss']
})
export class ControlTablaActuacionProcesoComponent implements OnInit {
  displayedColumns: string[] = ['fecha', 'numeroActuacion', 'actuacion', 'estadoRegistro', 'gestion'];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;
  dataTable: any[] = [
    
  ]

  @Input() defensaJudicialID:number;

  constructor(private router: Router, private defensaService:DefensaJudicialService,
    public dialog: MatDialog,) { }

  ngOnInit(): void {
    this.defensaService.getActuaciones(this.defensaJudicialID).subscribe(response =>{
      let i=1;
      response.forEach(element => {
        element.id=i;
        i++;
      });
      this.dataSource = new MatTableDataSource(response);
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.paginator._intl.itemsPerPageLabel = 'Elementos por página';
    });
    
  }
  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  };
  irNuevo() {
    this.router.navigate(['/gestionarProcesoDefensaJudicial/registrarActuacionProceso',this.defensaJudicialID]);
  }
  verDetalle(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/verDetalleActuacion',id]);
  }
  verDetalleEditar(id){
    this.router.navigate(['/gestionarProcesoDefensaJudicial/verDetalleEditarActuacionProceso',id]);
  }

  finalizarActuacion(id:number)
  {
    this.defensaService.finalizarActuacion(id).subscribe(response =>{
      this.openDialog("",response.message);
    });

  }
  eliminarActuacion(id:number)
  {
    this.openDialogSiNo("","¿Está seguro de eliminar este registro?",id);
  }

  eliminarActuacionConfirmado(id:number)
  {
    this.defensaService.eliminarActuacionJudicial(id).subscribe(response =>{
      this.openDialog("",response.message);
    });
  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    let dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
      if (result === true) {
        this.eliminarActuacionConfirmado(e);
      }
    });
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }
}
