﻿public class GoAdmit : GAction {
    public override bool PrePerform() {

        return true;
    }

    public override bool PostPerform() {

        Destroy(gameObject);
        return true;
    }
}
