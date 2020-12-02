import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { FaseDosAprobarConstruccionService } from 'src/app/core/_services/faseDosAprobarConstruccion/fase-dos-aprobar-construccion.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';

@Component({
  selector: 'app-hojas-vida-contratista-artc',
  templateUrl: './hojas-vida-contratista-artc.component.html',
  styleUrls: ['./hojas-vida-contratista-artc.component.scss']
})
export class HojasVidaContratistaArtcComponent implements OnInit {

  addressForm = this.fb.group({
    tieneObservaciones: [null, Validators.required],
    observaciones: [null, Validators.required],
    construccionPerfilObservacionId: []
  });
  editorStyle = {
    height: '100px'
  };

  config = {
    toolbar: [
      ['bold', 'italic', 'underline'],
      [{ list: 'ordered' }, { list: 'bullet' }],
      [{ indent: '-1' }, { indent: '+1' }],
      [{ align: [] }],
    ]
  };
  dataTablaHistorialObservacion: any[] = [];
  dataSource                 = new MatTableDataSource();
  displayedColumns: string[] = [
    'fechaRevision',
    'observacionesSupervision'
  ];

  @Input() observacionesCompleted;
  @Input() perfil: any;

  constructor(
    private dialog: MatDialog,
    private fb: FormBuilder,
    private faseDosAprobarConstruccionSvc: FaseDosAprobarConstruccionService )
  {}

  ngOnInit(): void {
    if (this.perfil){
      this.addressForm.get('tieneObservaciones')
        .setValue( this.perfil.tieneObservacionesSupervisor !== undefined ? this.perfil.tieneObservacionesSupervisor : null );
      this.addressForm.get('observaciones')
        .setValue( this.perfil.observacionSupervisor !== undefined ? this.perfil.observacionSupervisor.observacion : null );
      this.addressForm.get('construccionPerfilObservacionId')
        .setValue( this.perfil.observacionSupervisor !== undefined
          ? this.perfil.observacionSupervisor.construccionPerfilObservacionId : null );
    }
  }

  // getDataTable() {
  //   this.perfil.construccionPerfilObservacion.forEach( observacion => {} );
  // }

  maxLength(e: any, n: number) {
    if (e.editor.getLength() > n) {
      e.editor.deleteText(n, e.editor.getLength());
    }
  }

  textoLimpio(texto: string) {
    if ( texto !== undefined ) {
      const textolimpio = texto.replace(/<[^>]*>/g, '');
      return textolimpio.length;
    }
  }

  innerObservacion( observacion: string ) {
    if ( observacion !== undefined ) {
      const observacionHtml = observacion.replace( '"', '' );
      return observacionHtml;
    }
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data : { modalTitle, modalText }
    });
  }

  onSubmit(){

    const ConstruccionPerfil = {
      construccionPerfilId: this.perfil.construccionPerfilId,
      tieneObservacionesSupervisor: this.addressForm.value.tieneObservaciones,
      construccionPerfilObservacion: [
        {
          ConstruccionPerfilObservacionId: this.addressForm.value.construccionPerfilObservacionId,
          construccionPerfilId: this.perfil.construccionPerfilId,
          esSupervision: true,
          esActa: false,
          observacion: this.addressForm.value.observaciones
        }
      ]
    };

    console.log( ConstruccionPerfil );

    this.faseDosAprobarConstruccionSvc.createEditObservacionPerfilSupervisor( ConstruccionPerfil )
      .subscribe(
        response => this.openDialog( '', response.message ),
        err => this.openDialog( '', err.message )
      );

  }

}
