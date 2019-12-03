import { Directive, ElementRef, OnInit, Renderer2 } from '@angular/core';
import { SignalRService } from '../signal-r/signal-r.service';
@Directive({
    selector: '[IfConnected]'
})
export class IfSignalrDirective implements OnInit {

    constructor(
        private el: ElementRef,
        private renderer: Renderer2,
        private signalRService: SignalRService,
    ) { }

    ngOnInit() {
        this.signalRService.GetStatus().subscribe((res) => {
            if (this.signalRService.signalRConnected) {
                this.renderer.setStyle(this.el.nativeElement, 'color', 'green');
            } else {
                this.renderer.setStyle(this.el.nativeElement, 'color', 'yellow');
            }
        });
    }

}
