import { Component, Inject, OnInit } from '@angular/core';
import { FormControl, Validators } from '@angular/forms';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { FiduciaryCommitteeSessionService } from 'src/app/core/_services/fiduciaryCommitteeSession/fiduciary-committee-session.service';
import { ModalDialogComponent } from 'src/app/shared/components/modal-dialog/modal-dialog.component';
import { ComiteTecnico } from 'src/app/_interfaces/technicalCommitteSession';

@Component({
  selector: 'app-aplazar-sesion',
  templateUrl: './aplazar-sesion.component.html',
  styleUrls: ['./aplazar-sesion.component.scss']
})
export class AplazarSesionComponent implements OnInit {

  fechaAplazamiento: FormControl;
  minDate: Date;

  constructor(
                public dialogRef: MatDialogRef<AplazarSesionComponent>, 
                @Inject(MAT_DIALOG_DATA) public data: { 
                                            comite: ComiteTecnico, 
                                            //objetoComiteTecnico: ComiteTecnico 
                                          },
                private fiduciaryCommitteeSessionService: FiduciaryCommitteeSessionService,
                public dialog: MatDialog,
                private router: Router,
                
             ) 
  {
    this.declararFechaAplazamiento();
    this.minDate = new Date();
  }

  ngOnInit(): void {
  }

  private declararFechaAplazamiento() {
    this.fechaAplazamiento = new FormControl(null, [Validators.required]);
  }

  openDialog(modalTitle: string, modalText: string) {
    this.dialog.open(ModalDialogComponent, {
      width: '28em',
      data: { modalTitle, modalText }
    });
  }

  onSubmit(){

    let comiteTecnico: ComiteTecnico = {
      comiteTecnicoId: this.data.comite.comiteTecnicoId,
      fechaAplazamiento: this.fechaAplazamiento.value
    }

    this.fiduciaryCommitteeSessionService.aplazarSesionComite( comiteTecnico )
      .subscribe( respuesta => {
        this.openDialog( '', `<b>${respuesta.message}</b>` )
        if ( respuesta.code == "200" )
        {
          this.dialogRef.close();
          this.router.navigate(['/comiteFiduciario/registrarSesionDeComiteFiduciario', this.data.comite.comiteTecnicoId]);
        }
      })
    
  }

}
