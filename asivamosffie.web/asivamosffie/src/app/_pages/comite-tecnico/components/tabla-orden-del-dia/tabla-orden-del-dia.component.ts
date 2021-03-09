import { Component, OnInit, ViewChild } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { TechnicalCommitteSessionService } from 'src/app/core/_services/technicalCommitteSession/technical-committe-session.service';
import { Router } from '@angular/router';
import { EstadosComite, ComiteGrilla, ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';
import { MatDialog } from '@angular/material/dialog';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { CommonService, Dominio } from 'src/app/core/_services/common/common.service';
import { forkJoin } from 'rxjs';

@Component({
  selector: 'app-tabla-orden-del-dia',
  templateUrl: './tabla-orden-del-dia.component.html',
  styleUrls: ['./tabla-orden-del-dia.component.scss']
})
export class TablaOrdenDelDiaComponent implements OnInit {
  estadosComite = EstadosComite;
  listaMiembrosComite: Dominio[] = [];

  displayedColumns: string[] = ['fecha', 'numero', 'estado', 'id'];
  x = new MatTableDataSource();
  dataSource = new MatTableDataSource();

  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  @ViewChild(MatSort, { static: true }) sort: MatSort;

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

  constructor(
    private technicalCommitteeSessionService: TechnicalCommitteSessionService,
    private router: Router,
    public dialog: MatDialog,
    private commonService: CommonService
  ) {}

  ngOnInit(): void {
    forkJoin([
      this.technicalCommitteeSessionService.getListComiteGrilla()
      // this.commonService.listaMiembrosComiteTecnico(),
    ]).subscribe(response => {
      response[0].forEach(element => {
        element.fechaComite = element.fechaComite.split('T')[0].split('-').reverse().join('/');
      });
      this.dataSource = new MatTableDataSource(response[0]);
      this.listaMiembrosComite;
      this.initPaginator();
    });
  }
  initPaginator() {
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
      const endIndex = startIndex < length ? Math.min(startIndex + pageSize, length) : startIndex + pageSize;
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
    this.router.navigate(['/comiteTecnico/crearOrdenDelDia', e, '']);
  }

  onConvocar(e: number) {
    const comite: ComiteTecnico = {
      comiteTecnicoId: e,
      estadoComiteCodigo: this.estadosComite.convocada
    };

    this.technicalCommitteeSessionService.convocarComiteTecnico(comite).subscribe(respuesta => {
      this.openDialog('', `<b>${respuesta.message}</b>`);

      this.ngOnInit();
    });
  }

  openDialogSiNo(modalTitle: string, modalText: string, e: number) {
    const dialogRef = this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText, siNoBoton: true }
    });
    dialogRef.afterClosed().subscribe(result => {
      if (result === true) {
        this.OnDelete(e);
      }
    });
  }

  OnDelete(e: number) {
    this.technicalCommitteeSessionService.deleteComiteTecnicoByComiteTecnicoId(e).subscribe(respuesta => {
      this.openDialog('', `<b>${respuesta.message}</b>`);
      this.ngOnInit();
    });
  }
}
