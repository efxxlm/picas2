import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { Router } from '@angular/router';
import { EstadosComite, ComiteGrilla, ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';

@Component({
  selector: 'app-tabla-orden-del-dia',
  templateUrl: './tabla-orden-del-dia.component.html',
  styleUrls: ['./tabla-orden-del-dia.component.scss']
})
export class TablaOrdenDelDiaComponent implements OnInit {

  estadosComite = EstadosComite;

  displayedColumns: string[] = ['fecha', 'numero', 'estado', 'id'];
  dataSource = new MatTableDataSource();

  @ViewChild( MatPaginator, { static: true } ) paginator: MatPaginator;
  @ViewChild( MatSort, {static: true} ) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor ( private router: Router,
                public dialog: MatDialog,
                private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,            
                ) 
  {

  }

  ngOnInit(): void {

    this.fiduciaryCommitteeSessionService.getCommitteeSession()
      .subscribe( response => {
        
        this.dataSource = new MatTableDataSource( response );
      })

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

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onEdit(e: number) {
    this.router.navigate(['/comiteFiduciario/crearOrdenDelDia',e]);
  }

  onConvocar(e: number){

    let comite: ComiteTecnico = {
      comiteTecnicoId: e,
      estadoComiteCodigo: this.estadosComite.convocada
    }

    this.fiduciaryCommitteeSessionService.convocarComiteTecnico( comite )
      .subscribe( respuesta => {

        this.openDialog( ' sesión comité ', respuesta.message )

        this.ngOnInit();

      })
  }

  openDialogSiNo(modalTitle: string, modalText: string, e:number) {
    let dialogRef =this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton:true }
    });   
    dialogRef.afterClosed().subscribe(result => {
      if(result === true)
      {
        this.OnDelete(e)
      }           
    });
  }

  OnDelete(e: number){
    this.fiduciaryCommitteeSessionService.deleteComiteTecnicoByComiteTecnicoId( e )
      .subscribe( respuesta => {
        this.openDialog('', respuesta.message)
        this.ngOnInit();
      })
  }
}
